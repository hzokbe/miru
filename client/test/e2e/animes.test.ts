import { createPage, setup, url } from '@nuxt/test-utils/e2e';
import type { Page, Route } from 'playwright-core';
import { describe, expect, it } from 'vitest';
import { animeFixtures } from '../fixtures/animes.ts';

const path = '/animes';

const mockAnimes = async (
  page: Page,
  handler: (route: Route) => Promise<void>,
) => {
  await page.route(
    (target) => target.pathname.endsWith('/animes'),
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

  await mockAnimes(page, handler);

  await page.goto(url(path), { waitUntil: 'domcontentloaded' });

  return page;
};

describe('animes page', async () => {
  await setup({
    browser: true,
    nuxtConfig: { ssr: false },
  });

  it('renders one card per anime', async () => {
    const page = await openPage((route) =>
      route.fulfill({ json: animeFixtures }),
    );

    await page.getByRole('heading', { level: 3 }).first().waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(
      animeFixtures.length,
    );

    expect(await page.getByText('Top animes').isVisible()).toBe(true);

    await page.close();
  });

  it('shows the first anime title and badges', async () => {
    const page = await openPage((route) =>
      route.fulfill({ json: animeFixtures }),
    );

    const first = animeFixtures[0]!;

    await page.getByTestId('anime-score').first().waitFor();

    expect(
      await page
        .getByRole('heading', { name: first.title, exact: true })
        .count(),
    ).toBe(1);

    expect(await page.getByTestId('anime-score').first().innerText()).toContain(
      '8.4',
    );

    expect(await page.getByTestId('anime-type').count()).toBe(
      animeFixtures.length,
    );

    await page.close();
  });

  it('shows skeletons while loading and cards afterwards', async () => {
    const page = await openPage(async (route) => {
      await new Promise((resolve) => setTimeout(resolve, 1500));

      await route.fulfill({ json: animeFixtures });
    });

    await page.locator('h3').first().waitFor({ state: 'attached' });

    expect(await page.locator('h3').count()).toBe(9);

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    expect(await page.getByTestId('anime-score').count()).toBe(0);

    await page.getByTestId('anime-score').first().waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(
      animeFixtures.length,
    );

    await page.close();
  });

  it('shows the empty state', async () => {
    const page = await openPage((route) => route.fulfill({ json: [] }));

    await page.getByText('No anime found').waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    await page.close();
  });

  it('shows the error state', async () => {
    const page = await openPage((route) =>
      route.fulfill({ status: 500, json: { message: 'error' } }),
    );

    await page
      .getByText('Something went wrong while loading the anime list.')
      .waitFor();

    expect(await page.getByText('No anime found').count()).toBe(0);

    await page.close();
  });

  it('navigates to the anime page when a card is clicked', async () => {
    const page = await openPage((route) =>
      route.fulfill({ json: animeFixtures }),
    );

    const target = animeFixtures[3]!;

    await page.getByText(target.title, { exact: true }).click();

    await page.waitForURL(`**/animes/${target.id}`);

    expect(page.url()).toContain(`/animes/${target.id}`);

    await page.close();
  });
});
