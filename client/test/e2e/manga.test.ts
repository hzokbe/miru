import { createPage, setup, url } from '@nuxt/test-utils/e2e';
import type { Page, Route } from 'playwright-core';
import { describe, expect, it } from 'vitest';
import { mangaFixtures } from '../fixtures/mangas.ts';

const manga = mangaFixtures[0]!;

const detailPath = `/mangas/${manga.id}`;

type Handler = (route: Route) => Promise<void>;

const mockApi = async (page: Page, routes: Record<string, Handler>) => {
  await page.route(
    (target) =>
      Object.keys(routes).some((suffix) => target.pathname.endsWith(suffix)),
    async (route) => {
      if (route.request().resourceType() === 'document') {
        return route.fallback();
      }

      const pathname = new URL(route.request().url()).pathname;

      const suffix = Object.keys(routes).find((key) => pathname.endsWith(key))!;

      return routes[suffix]!(route);
    },
  );
};

const openPage = async (path: string, routes: Record<string, Handler>) => {
  const page = await createPage();

  await mockApi(page, routes);

  await page.goto(url(path), { waitUntil: 'domcontentloaded' });

  return page;
};

describe('manga page', async () => {
  await setup({
    browser: true,
    nuxtConfig: { ssr: false },
  });

  it('renders title, synopsis and badges', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) => route.fulfill({ json: manga }),
    });

    await page.getByTestId('manga-score').waitFor();

    expect(
      await page
        .getByRole('heading', { level: 3, name: manga.title, exact: true })
        .count(),
    ).toBe(1);

    expect(await page.getByText(manga.synopsis!).isVisible()).toBe(true);

    expect(await page.getByTestId('manga-score').innerText()).toContain(
      String(manga.score),
    );

    expect(await page.getByTestId('manga-chapters').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-volumes').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-status').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-published').isVisible()).toBe(true);

    await page.close();
  });

  it('sets the page title from the manga', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) => route.fulfill({ json: manga }),
    });

    await page.getByTestId('manga-score').waitFor();

    await page.waitForFunction(
      (title) => document.title.includes(title),
      manga.title,
    );

    expect(await page.title()).toContain(manga.title);

    await page.close();
  });

  it('shows the skeleton while loading and the content afterwards', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: async (route) => {
        await new Promise((resolve) => setTimeout(resolve, 1500));

        await route.fulfill({ json: manga });
      },
    });

    await page.locator('h3').first().waitFor({ state: 'attached' });

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    expect(await page.getByTestId('manga-score').count()).toBe(0);

    await page.getByTestId('manga-score').waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(1);

    await page.close();
  });

  it('shows the fallback description when there is no synopsis', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) =>
        route.fulfill({ json: { ...manga, synopsis: null } }),
    });

    await page.getByText('No synopsis').waitFor();

    expect(await page.getByText('No synopsis').isVisible()).toBe(true);

    await page.close();
  });

  it('shows the not found state', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) =>
        route.fulfill({ status: 404, json: { message: 'not found' } }),
    });

    await page.getByText('Manga not found').waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    expect(await page.getByTestId('manga-score').count()).toBe(0);

    await page.close();
  });

  it('shows the error state', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) =>
        route.fulfill({ status: 500, json: { message: 'error' } }),
    });

    await page
      .getByText(
        'Something went wrong while loading the manga details. Please try again.',
      )
      .waitFor();

    expect(await page.getByText('Manga not found').count()).toBe(0);

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    await page.close();
  });

  it('opens the manga page from the list', async () => {
    const target = mangaFixtures[3]!;

    const page = await openPage('/mangas', {
      '/mangas': (route) => route.fulfill({ json: mangaFixtures }),
      [`/mangas/${target.id}`]: (route) => route.fulfill({ json: target }),
    });

    await page.getByText(target.title, { exact: true }).click();

    await page.waitForURL(`**/mangas/${target.id}`);

    await page.getByTestId('manga-score').waitFor();

    expect(
      await page
        .getByRole('heading', { level: 3, name: target.title, exact: true })
        .count(),
    ).toBe(1);

    await page.close();
  });
});
