import { fileURLToPath } from 'node:url';
import { afterEach, describe, expect, it } from 'vitest';
import { createPage, setup } from '@nuxt/test-utils/e2e';
import type { Page } from 'playwright-core';

describe('mangas (real API)', async () => {
  await setup({
    rootDir: fileURLToPath(new URL('../..', import.meta.url)),
    browser: true,
  });

  let page: Page;

  afterEach(async () => {
    await page.close();
  });

  it('lists the top mangas', async () => {
    page = await createPage('/mangas');

    const section = page.getByRole('heading', { level: 2, name: 'Top mangas' });

    await section.waitFor({ state: 'visible' });

    const cardTitles = page.getByRole('heading', { level: 3 });

    await cardTitles.first().waitFor({ state: 'visible' });

    expect(await cardTitles.count()).toBeGreaterThan(0);
  });

  it('navigates to the mangas page from the header', async () => {
    page = await createPage('/');

    await page.getByRole('link', { name: 'Mangas' }).first().click();

    await page.waitForURL('**/mangas');

    const section = page.getByRole('heading', { level: 2, name: 'Top mangas' });

    await section.waitFor({ state: 'visible' });

    expect(page.url()).toMatch(/\/mangas$/);
  });

  it('opens the manga page when a card is clicked', async () => {
    page = await createPage('/mangas');

    const firstTitle = page.getByRole('heading', { level: 3 }).first();

    await firstTitle.waitFor({ state: 'visible' });

    await firstTitle.click();

    await page.waitForURL(/\/mangas\/[0-9a-f-]{36}$/);

    expect(page.url()).toMatch(/\/mangas\/[0-9a-f-]{36}$/);
  });

  it('shows the error state and recovers after retrying', async () => {
    page = await createPage('/');

    const blockListRequest = (
      route: Parameters<Parameters<Page['route']>[1]>[0],
    ) =>
      route.request().resourceType() === 'fetch'
        ? route.abort()
        : route.continue();

    await page.route('**/mangas', blockListRequest);

    await page.getByRole('link', { name: 'Mangas' }).first().click();

    const errorTitle = page.getByText('Unable to load mangas', { exact: true });

    await errorTitle.waitFor({ state: 'visible' });

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    await page.unroute('**/mangas', blockListRequest);

    await page.getByRole('button', { name: 'Try again' }).click();

    const firstTitle = page.getByRole('heading', { level: 3 }).first();

    await firstTitle.waitFor({ state: 'visible' });

    expect(await errorTitle.count()).toBe(0);
  });
});
