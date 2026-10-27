import { createPage, setup, url } from '@nuxt/test-utils/e2e';
import type { Page, Route } from 'playwright-core';
import { describe, expect, it } from 'vitest';
import { characterFixtures } from '../fixtures/characters.ts';

const character = characterFixtures[1]!;

const fullName = 'Miyuki Shirogane';

const detailPath = `/characters/${character.id}`;

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

describe('character page', async () => {
  await setup({
    browser: true,
    nuxtConfig: { ssr: false },
  });

  it('renders name, description and badges', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) => route.fulfill({ json: character }),
    });

    await page.getByTestId('character-role').waitFor();

    expect(
      await page
        .getByRole('heading', { level: 3, name: fullName, exact: true })
        .count(),
    ).toBe(1);

    expect(await page.getByText(character.description!).isVisible()).toBe(true);

    expect(await page.getByTestId('character-role').innerText()).toContain(
      character.role,
    );

    expect(await page.getByTestId('character-gender').innerText()).toContain(
      character.gender,
    );

    expect(await page.getByTestId('character-age').innerText()).toContain(
      character.age!,
    );

    await page.close();
  });

  it('sets the page title from the character', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) => route.fulfill({ json: character }),
    });

    await page.getByTestId('character-role').waitFor();

    await page.waitForFunction(
      (title) => document.title.includes(title),
      fullName,
    );

    expect(await page.title()).toContain(fullName);

    await page.close();
  });

  it('shows the skeleton while loading and the content afterwards', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: async (route) => {
        await new Promise((resolve) => setTimeout(resolve, 1500));

        await route.fulfill({ json: character });
      },
    });

    await page.locator('h3').first().waitFor({ state: 'attached' });

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    expect(await page.getByTestId('character-role').count()).toBe(0);

    await page.getByTestId('character-role').waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(1);

    await page.close();
  });

  it('shows the placeholder age and fallback description', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) =>
        route.fulfill({
          json: { ...character, age: null, description: null },
        }),
    });

    await page.getByText('No description').waitFor();

    expect(await page.getByTestId('character-age').innerText()).toContain('?');

    await page.close();
  });

  it('shows the not found state', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) =>
        route.fulfill({ status: 404, json: { message: 'not found' } }),
    });

    await page.getByText('Character not found').waitFor();

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    expect(await page.getByTestId('character-role').count()).toBe(0);

    await page.close();
  });

  it('shows the error state', async () => {
    const page = await openPage(detailPath, {
      [detailPath]: (route) =>
        route.fulfill({ status: 500, json: { message: 'error' } }),
    });

    await page
      .getByText(
        'Something went wrong while loading the character details. Please try again.',
      )
      .waitFor();

    expect(await page.getByText('Character not found').count()).toBe(0);

    expect(await page.getByRole('heading', { level: 3 }).count()).toBe(0);

    await page.close();
  });
});
