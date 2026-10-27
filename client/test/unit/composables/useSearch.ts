import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defineComponent, h } from 'vue';
import { animeFixtures } from '~~/test/fixtures/animes.ts';
import { characterFixtures } from '~~/test/fixtures/characters.ts';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

const response = {
  animes: animeFixtures.slice(0, 3),
  mangas: mangaFixtures.slice(0, 3),
  characters: characterFixtures.slice(0, 3),
};

const mountComposable = async () => {
  let result!: ReturnType<typeof useSearch>;

  await mountSuspended(
    defineComponent({
      setup() {
        result = useSearch();

        return () => h('div');
      },
    }),
  );

  return result;
};

describe('useSearch', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();

    clearNuxtState('query');
  });

  it('requests the search endpoint with the current query', async () => {
    useQuery().setQuery('kaguya');

    apiMock.mockResolvedValue(response);

    const { status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledWith('/search/kaguya');
  });

  it('exposes the fetched results', async () => {
    useQuery().setQuery('kaguya');

    apiMock.mockResolvedValue(response);

    const { search, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(search.value).toEqual(response);
  });

  it('defaults to null while loading', async () => {
    useQuery().setQuery('kaguya');

    apiMock.mockReturnValue(new Promise(() => {}));

    const { search } = await mountComposable();

    expect(search.value).toBeNull();
  });

  it('exposes the error and keeps null on failure', async () => {
    useQuery().setQuery('kaguya');

    apiMock.mockRejectedValue(new Error('request failed'));

    const { search, error, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(search.value).toBeNull();
  });

  it('refetches on refresh', async () => {
    useQuery().setQuery('kaguya');

    apiMock.mockResolvedValueOnce({ ...response, animes: [] });

    apiMock.mockResolvedValueOnce(response);

    const { search, refresh, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(search.value?.animes).toHaveLength(0);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(search.value).toEqual(response);
  });
});
