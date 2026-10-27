import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import type { VueWrapper } from '@vue/test-utils';
import { DOMWrapper } from '@vue/test-utils';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { SearchDrawer } from '#components';
import { animeFixtures } from '~~/test/fixtures/animes.ts';
import { characterFixtures } from '~~/test/fixtures/characters.ts';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';

const { apiMock, navigateToMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
  navigateToMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('navigateTo', () => navigateToMock);

const fullResponse = {
  animes: animeFixtures,
  mangas: mangaFixtures,
  characters: characterFixtures,
};

const onlyAnimes = {
  animes: animeFixtures.slice(0, 2),
  mangas: [],
  characters: [],
};

const getInput = () => document.body.querySelector('input')!;

let wrapper: VueWrapper | undefined;

const openDrawer = async () => {
  wrapper = await mountSuspended(SearchDrawer, {
    attachTo: document.body,
  });

  await wrapper.find('button').trigger('click');

  await vi.waitFor(() => expect(getInput()).not.toBeNull());

  return wrapper;
};

const type = async (value: string) => {
  await new DOMWrapper(getInput()).setValue(value);
};

const pressEnter = async () => {
  await new DOMWrapper(getInput()).trigger('keydown.enter');
};

describe('SearchDrawer', () => {
  beforeEach(() => {
    apiMock.mockReset();

    navigateToMock.mockReset();

    clearNuxtData();

    clearNuxtState('query');
  });

  afterEach(() => {
    wrapper?.unmount();

    wrapper = undefined;

    document.body.innerHTML = '';
  });

  it('opens the command palette with the placeholder', async () => {
    await openDrawer();

    expect(getInput().getAttribute('placeholder')).toBe(
      'Search animes, mangas or characters',
    );
  });

  it('does not call the API for an empty query', async () => {
    await openDrawer();

    await type('   ');

    await new Promise((resolve) => setTimeout(resolve, 500));

    expect(apiMock).not.toHaveBeenCalled();
  });

  it('calls the API with the trimmed term after the debounce', async () => {
    apiMock.mockResolvedValue(onlyAnimes);

    await openDrawer();

    await type(' kaguya ');

    await vi.waitFor(() =>
      expect(apiMock).toHaveBeenCalledWith('/search/kaguya'),
    );
  });

  it('renders the suggestions grouped by type', async () => {
    apiMock.mockResolvedValue(fullResponse);

    await openDrawer();

    await type('kaguya');

    await vi.waitFor(() => {
      const text = document.body.textContent;

      expect(text).toContain('Animes');

      expect(text).toContain('Mangas');

      expect(text).toContain('Characters');

      expect(text).toContain(animeFixtures[0]!.title);

      expect(text).toContain(mangaFixtures[0]!.title);

      expect(text).toContain(characterFixtures[0]!.name);
    });
  });

  it('limits each group to five items', async () => {
    apiMock.mockResolvedValue(fullResponse);

    await openDrawer();

    await type('kaguya');

    await vi.waitFor(() =>
      expect(document.body.textContent).toContain(animeFixtures[4]!.title),
    );

    expect(document.body.textContent).not.toContain(animeFixtures[5]!.title);

    expect(document.body.textContent).not.toContain(mangaFixtures[5]!.title);
  });

  it('hides groups without items', async () => {
    apiMock.mockResolvedValue(onlyAnimes);

    await openDrawer();

    await type('kaguya');

    await vi.waitFor(() =>
      expect(document.body.textContent).toContain(animeFixtures[0]!.title),
    );

    expect(document.body.textContent).not.toContain('Mangas');

    expect(document.body.textContent).not.toContain('Characters');
  });

  it('navigates to the search page on enter', async () => {
    apiMock.mockResolvedValue(onlyAnimes);

    await openDrawer();

    await type(' kaguya ');

    await vi.waitFor(() => expect(useQuery().query.value).toBe(' kaguya '));

    await pressEnter();

    await vi.waitFor(() =>
      expect(navigateToMock).toHaveBeenCalledWith('/search/kaguya'),
    );
  });

  it('does not navigate on enter with an empty query', async () => {
    await openDrawer();

    await type('   ');

    await pressEnter();

    await new Promise((resolve) => setTimeout(resolve, 100));

    expect(navigateToMock).not.toHaveBeenCalled();
  });

  it('clears the query when the drawer closes', async () => {
    apiMock.mockResolvedValue(onlyAnimes);

    await openDrawer();

    await type('kaguya');

    await vi.waitFor(() => expect(useQuery().query.value).toBe('kaguya'));

    await pressEnter();

    await vi.waitFor(() => expect(useQuery().query.value).toBe(''));
  });
});
