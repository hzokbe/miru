import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defineComponent, h } from 'vue';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

const manga = mangaFixtures[0]!;

const mountComposable = async (id = String(manga.id)) => {
  let result!: ReturnType<typeof useManga>;

  let initialLoading = false;

  await mountSuspended(
    defineComponent({
      setup() {
        result = useManga(id);

        initialLoading = result.loading.value;

        return () => h('div');
      },
    }),
  );

  return { ...result, initialLoading };
};

describe('useManga', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('requests the manga endpoint with the id', async () => {
    apiMock.mockResolvedValue(manga);

    const { status } = await mountComposable('7');

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledWith('/mangas/7');
  });

  it('exposes the fetched manga', async () => {
    apiMock.mockResolvedValue(manga);

    const { manga: data, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value).toEqual(manga);
  });

  it('defaults to null while loading', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { manga: data } = await mountComposable();

    expect(data.value).toBeNull();
  });

  it('is loading while pending and stops after success', async () => {
    let resolve!: (value: typeof manga) => void;

    apiMock.mockReturnValue(
      new Promise<typeof manga>((r) => {
        resolve = r;
      }),
    );

    const { initialLoading, loading } = await mountComposable();

    expect(initialLoading).toBe(true);

    resolve(manga);

    await vi.waitFor(() => expect(loading.value).toBe(false));
  });

  it('returns null without error when the API responds 404', async () => {
    apiMock.mockRejectedValue({ statusCode: 404 });

    const { manga: data, error, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value).toBeNull();

    expect(error.value).toBeUndefined();
  });

  it('exposes the error and keeps null on other failures', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const { manga: data, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(data.value).toBeNull();

    expect(loading.value).toBe(false);
  });

  it('refetches on refresh', async () => {
    apiMock.mockResolvedValueOnce({ ...manga, title: 'first' });

    apiMock.mockResolvedValueOnce(manga);

    const { manga: data, refresh, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value?.title).toBe('first');

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(data.value).toEqual(manga);
  });
});
