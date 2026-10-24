import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defineComponent, h } from 'vue';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

const mountComposable = async () => {
  let result!: ReturnType<typeof useMangas>;

  let initialLoading = false;

  await mountSuspended(
    defineComponent({
      setup() {
        result = useMangas();

        initialLoading = result.loading.value;

        return () => h('div');
      },
    }),
  );

  return { ...result, initialLoading };
};

describe('useMangas', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('requests the mangas endpoint', async () => {
    apiMock.mockResolvedValue(mangaFixtures);

    const { status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledWith('/mangas');
  });

  it('exposes the fetched mangas', async () => {
    apiMock.mockResolvedValue(mangaFixtures);

    const { mangas, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(mangas.value).toEqual(mangaFixtures);
  });

  it('defaults to an empty list while loading', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { mangas } = await mountComposable();

    expect(mangas.value).toEqual([]);
  });

  it('is loading while pending and stops after success', async () => {
    let resolve!: (value: typeof mangaFixtures) => void;

    apiMock.mockReturnValue(
      new Promise<typeof mangaFixtures>((r) => {
        resolve = r;
      }),
    );

    const { initialLoading, loading } = await mountComposable();

    expect(initialLoading).toBe(true);

    resolve(mangaFixtures);

    await vi.waitFor(() => expect(loading.value).toBe(false));
  });

  it('exposes the error and keeps the default list on failure', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const { mangas, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(mangas.value).toEqual([]);

    expect(loading.value).toBe(false);
  });

  it('refetches on refresh', async () => {
    apiMock.mockResolvedValueOnce([mangaFixtures[0]]);

    apiMock.mockResolvedValueOnce(mangaFixtures);

    const { mangas, refresh, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(mangas.value).toHaveLength(1);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(mangas.value).toEqual(mangaFixtures);
  });
});
