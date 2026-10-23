import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defineComponent, h } from 'vue';
import { animeFixtures } from '~~/test/fixtures/animes.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

const mountComposable = async () => {
  let result!: ReturnType<typeof useAnimes>;

  let initialLoading = false;

  await mountSuspended(
    defineComponent({
      setup() {
        result = useAnimes();

        initialLoading = result.loading.value;

        return () => h('div');
      },
    }),
  );

  return { ...result, initialLoading };
};

describe('useAnimes', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('requests the animes endpoint', async () => {
    apiMock.mockResolvedValue(animeFixtures);

    const { status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledWith('/animes');
  });

  it('exposes the fetched animes', async () => {
    apiMock.mockResolvedValue(animeFixtures);

    const { animes, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(animes.value).toEqual(animeFixtures);
  });

  it('defaults to an empty list while loading', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { animes } = await mountComposable();

    expect(animes.value).toEqual([]);
  });

  it('is loading while pending and stops after success', async () => {
    let resolve!: (value: typeof animeFixtures) => void;

    apiMock.mockReturnValue(
      new Promise<typeof animeFixtures>((r) => {
        resolve = r;
      }),
    );

    const { initialLoading, loading } = await mountComposable();

    expect(initialLoading).toBe(true);

    resolve(animeFixtures);

    await vi.waitFor(() => expect(loading.value).toBe(false));
  });

  it('exposes the error and keeps the default list on failure', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const { animes, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(animes.value).toEqual([]);

    expect(loading.value).toBe(false);
  });

  it('refetches on refresh', async () => {
    apiMock.mockResolvedValueOnce([animeFixtures[0]]);

    apiMock.mockResolvedValueOnce(animeFixtures);

    const { animes, refresh, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(animes.value).toHaveLength(1);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(animes.value).toEqual(animeFixtures);
  });
});
