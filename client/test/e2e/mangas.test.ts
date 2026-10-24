import { createPage, setup, url } from '@nuxt/test-utils/e2e';
import type { Page, Route } from 'playwright-core';
import { describe, expect, it } from 'vitest';
import { mangaFixtures } from '../fixtures/mangas.ts';

const path = '/mangas';

const mockMangas = async (
  page: Page,
  handler: (route: Route) => Promise<void>,
) => {
  await page.route(
    (target) => target.pathname.endsWith('/mangas'),
    async (route) => {
      if (route.request().resourceType() === 'document') {
        return route.fallback();
      }

      return handler(route);
    },
  );
};

const openPage = async (handler: (route: Route) => Promise<void>) => {
  const page = await createPage();

  await mockMangas(page, handler);

  await page.goto(url(path), { waitUntil: 'domcontentloaded' });

  return page;
};

describe('mangas page', async () => {
  await setup({
    browser: true,
    nuxtConfig: { ssr: false },
  });

  it('renders one card per manga', async () => {
    const page = await openPage((route) =>
      route.fulfill({ json: mangaFixtures }),
    );

    await page.getByRole('heading', { level: 3 }).first().waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(
      mangaFixtures.length,
    );

    expect(await page.getByText('Top mangas').isVisible()).toBe(true);

    await page.close();
  });

  it('shows the first manga title and badges', async () => {
    const page = await openPage((route) =>
      route.fulfill({ json: mangaFixtures }),
    );

    const first = mangaFixtures[0]!;

    await page.getByTestId('manga-score').first().waitFor();

    expect(
      await page
        .getByRole('heading', { name: first.title, exact: true })
        .count(),
    ).toBe(1);

    expect(await page.getByTestId('manga-score').first().innerText()).toContain(
      String(first.score),
    );

    expect(await page.getByTestId('manga-volumes').count()).toBe(
      mangaFixtures.length,
    );

    expect(await page.getByTestId('manga-chapters').count()).toBe(
      mangaFixtures.length,
    );

    await page.close();
  });

  it('shows skeletons while loading and cards afterwards', async () => {
    const page = await openPage(async (route) => {
      await new Promise((resolve) => setTimeout(resolve, 1500));

      await route.fulfill({ json: mangaFixtures });
    });

    await page.locator('h3').first().waitFor({ state: 'attached' });

    expect(await page.locator('h3').count()).toBe(9);

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    expect(await page.getByTestId('manga-score').count()).toBe(0);

    await page.getByTestId('manga-score').first().waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(
      mangaFixtures.length,
    );

    await page.close();
  });

  it('shows the empty state', async () => {
    const page = await openPage((route) => route.fulfill({ json: [] }));

    await page.getByText('No manga found').waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    await page.close();
  });

  it('shows the error state', async () => {
    const page = await openPage((route) =>
      route.fulfill({ status: 500, json: { message: 'error' } }),
    );

    await page
      .getByText('Something went wrong while loading the manga list.')
      .waitFor();

    expect(await page.getByText('No manga found').count()).toBe(0);

    await page.close();
  });

  it('navigates to the manga page when a card is clicked', async () => {
    const page = await openPage((route) =>
      route.fulfill({ json: mangaFixtures }),
    );

    const target = mangaFixtures[3]!;

    await page.getByText(target.title, { exact: true }).click();

    await page.waitForURL(`**/mangas/${target.id}`);

    expect(page.url()).toContain(`/mangas/${target.id}`);

    await page.close();
  });
});
