import { fileURLToPath } from 'node:url';
import { afterEach, describe, expect, it, vi } from 'vitest';
import { createPage, setup } from '@nuxt/test-utils/e2e';
import type { Page } from 'playwright-core';

describe('anime details (real API)', async () => {
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

  const getAnimeHeading = (title: string) =>
    page.getByRole('heading', { level: 2, name: title, exact: true });

  it('shows the details of the clicked anime', async () => {
    page = await createPage('/animes');

    const { firstTitle, title } = await getFirstTitle();

    await firstTitle.click();

    await page.waitForURL(/\/animes\/[0-9a-f-]{36}$/);

    await getAnimeHeading(title).waitFor({ state: 'visible' });

    expect(await page.getByTestId('anime-score').isVisible()).toBe(true);

    expect(await page.getByTestId('anime-episodes').isVisible()).toBe(true);

    expect(await page.getByTestId('anime-status').isVisible()).toBe(true);

    expect(await page.getByTestId('anime-aired').isVisible()).toBe(true);
  }, 30000);

  it('sets the document title to the anime title', async () => {
    page = await createPage('/animes');

    const { firstTitle, title } = await getFirstTitle();

    await firstTitle.click();

    await page.waitForURL(/\/animes\/[0-9a-f-]{36}$/);

    await getAnimeHeading(title).waitFor({ state: 'visible' });

    await vi.waitFor(async () => expect(await page.title()).toContain(title), {
      timeout: 10000,
    });
  }, 30000);

  it('shows the error state and recovers after retrying', async () => {
    page = await createPage('/animes');

    const { firstTitle, title } = await getFirstTitle();

    const blockDetailRequest = (
      route: Parameters<Parameters<Page['route']>[1]>[0],
    ) =>
      route.request().resourceType() === 'fetch'
        ? route.abort()
        : route.continue();

    await page.route('**/animes/*', blockDetailRequest);

    await firstTitle.click();

    const errorTitle = page.getByText('Unable to load anime', { exact: true });

    await errorTitle.waitFor({ state: 'visible' });

    expect(await getAnimeHeading(title).count()).toBe(0);

    await page.unroute('**/animes/*', blockDetailRequest);

    await page.getByRole('button', { name: 'Try again' }).click();

    await getAnimeHeading(title).waitFor({ state: 'visible' });

    expect(await errorTitle.count()).toBe(0);
  }, 30000);
});
