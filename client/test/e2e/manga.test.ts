import { fileURLToPath } from 'node:url';
import { afterEach, describe, expect, it, vi } from 'vitest';
import { createPage, setup } from '@nuxt/test-utils/e2e';
import type { Page } from 'playwright-core';

describe('manga details (real API)', async () => {
  await setup({
    rootDir: fileURLToPath(new URL('../..', import.meta.url)),
    browser: true,
  });

  let page: Page;

  afterEach(async () => {
    await page.close();
  });

  const getFirstTitle = async () => {
    const firstTitle = page.getByRole('heading', { level: 3 }).first();

    await firstTitle.waitFor({ state: 'visible' });

    const title = ((await firstTitle.textContent()) ?? '').trim();

    return { firstTitle, title };
  };

  const getMangaHeading = (title: string) =>
    page.getByRole('heading', { level: 2, name: title, exact: true });

  const openFirstManga = async () => {
    page = await createPage('/mangas');

    const { firstTitle, title } = await getFirstTitle();

    await firstTitle.click();

    await page.waitForURL(/\/mangas\/[0-9a-f-]{36}$/);

    await getMangaHeading(title).waitFor({ state: 'visible' });

    return title;
  };

  it('shows the details of the clicked manga', async () => {
    await openFirstManga();

    expect(await page.getByTestId('manga-score').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-chapters').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-volumes').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-status').isVisible()).toBe(true);

    expect(await page.getByTestId('manga-published').isVisible()).toBe(true);
  }, 30000);

  it('shows the characters section of the clicked manga', async () => {
    await openFirstManga();

    const charactersTitle = page.getByText('Characters', { exact: true });

    await charactersTitle.waitFor({ state: 'visible' });

    expect(await charactersTitle.isVisible()).toBe(true);
  }, 30000);

  it('sets the document title to the manga title', async () => {
    const title = await openFirstManga();

    await vi.waitFor(async () => expect(await page.title()).toContain(title), {
      timeout: 10000,
    });
  }, 30000);

  it('shows the error state and recovers after retrying', async () => {
    page = await createPage('/mangas');

    const { firstTitle, title } = await getFirstTitle();

    const blockDetailRequest = (
      route: Parameters<Parameters<Page['route']>[1]>[0],
    ) =>
      route.request().resourceType() === 'fetch'
        ? route.abort()
        : route.continue();

    await page.route('**/mangas/*', blockDetailRequest);

    await firstTitle.click();

    const errorTitle = page.getByText('Unable to load manga', { exact: true });

    await errorTitle.waitFor({ state: 'visible' });

    expect(await getMangaHeading(title).count()).toBe(0);

    expect(await page.getByText('Characters', { exact: true }).count()).toBe(0);

    await page.unroute('**/mangas/*', blockDetailRequest);

    await page.getByRole('button', { name: 'Try again' }).click();

    await getMangaHeading(title).waitFor({ state: 'visible' });

    expect(await errorTitle.count()).toBe(0);
  }, 30000);
});
