import { fileURLToPath } from 'node:url';
import { afterEach, describe, expect, it } from 'vitest';
import { createPage, setup } from '@nuxt/test-utils/e2e';
import type { Page } from 'playwright-core';

describe('animes (real API)', async () => {
  await setup({
    rootDir: fileURLToPath(new URL('../..', import.meta.url)),
    browser: true,
  });

  let page: Page;

  afterEach(async () => {
    await page.close();
  });

  it('shows the connected toast when the app loads', async () => {
    page = await createPage('/');

    const toast = page.getByText('The API is online and responding normally.', {
      exact: true,
    });

    await toast.waitFor({ state: 'visible' });

    expect(await page.getByText('Connected', { exact: true }).isVisible()).toBe(
      true,
    );

    expect(
      await page
        .getByText('Unable to reach the server', { exact: true })
        .count(),
    ).toBe(0);
  });

  it('lists the top animes', async () => {
    page = await createPage('/animes');

    const section = page.getByRole('heading', { level: 2, name: 'Top animes' });

    await section.waitFor({ state: 'visible' });

    const cardTitles = page.getByRole('heading', { level: 3 });

    await cardTitles.first().waitFor({ state: 'visible' });

    expect(await cardTitles.count()).toBeGreaterThan(0);
  });

  it('navigates to the animes page from the header', async () => {
    page = await createPage('/');

    await page.getByRole('link', { name: 'Animes' }).first().click();

    await page.waitForURL('**/animes');

    const section = page.getByRole('heading', { level: 2, name: 'Top animes' });

    await section.waitFor({ state: 'visible' });

    expect(page.url()).toMatch(/\/animes$/);
  });

  it('opens the anime page when a card is clicked', async () => {
    page = await createPage('/animes');

    const firstTitle = page.getByRole('heading', { level: 3 }).first();

    await firstTitle.waitFor({ state: 'visible' });

    await firstTitle.click();

    await page.waitForURL(/\/animes\/[0-9a-f-]{36}$/);

    expect(page.url()).toMatch(/\/animes\/[0-9a-f-]{36}$/);
  });

  it('shows the error state and recovers after retrying', async () => {
    page = await createPage('/');

    const blockListRequest = (
      route: Parameters<Parameters<Page['route']>[1]>[0],
    ) =>
      route.request().resourceType() === 'fetch'
        ? route.abort()
        : route.continue();

    await page.route('**/animes', blockListRequest);

    await page.getByRole('link', { name: 'Animes' }).first().click();

    const errorTitle = page.getByText('Unable to load animes', { exact: true });

    await errorTitle.waitFor({ state: 'visible' });

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    await page.unroute('**/animes', blockListRequest);

    await page.getByRole('button', { name: 'Try again' }).click();

    const firstTitle = page.getByRole('heading', { level: 3 }).first();

    await firstTitle.waitFor({ state: 'visible' });

    expect(await errorTitle.count()).toBe(0);
  });
});
