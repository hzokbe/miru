import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defineComponent, h } from 'vue';
import { animeFixtures } from '~~/test/fixtures/animes.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

const anime = animeFixtures[0]!;

const mountComposable = async (id = String(anime.id)) => {
  let result!: ReturnType<typeof useAnime>;

  let initialLoading = false;

  await mountSuspended(
    defineComponent({
      setup() {
        result = useAnime(id);

        initialLoading = result.loading.value;

        return () => h('div');
      },
    }),
  );

  return { ...result, initialLoading };
};

describe('useAnime', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('requests the anime endpoint with the id', async () => {
    apiMock.mockResolvedValue(anime);

    const { status } = await mountComposable('7');

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledWith('/animes/7');
  });

  it('exposes the fetched anime', async () => {
    apiMock.mockResolvedValue(anime);

    const { anime: data, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value).toEqual(anime);
  });

  it('defaults to null while loading', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { anime: data } = await mountComposable();

    expect(data.value).toBeNull();
  });

  it('is loading while pending and stops after success', async () => {
    let resolve!: (value: typeof anime) => void;

    apiMock.mockReturnValue(
      new Promise<typeof anime>((r) => {
        resolve = r;
      }),
    );

    const { initialLoading, loading } = await mountComposable();

    expect(initialLoading).toBe(true);

    resolve(anime);

    await vi.waitFor(() => expect(loading.value).toBe(false));
  });

  it('returns null without error when the API responds 404', async () => {
    apiMock.mockRejectedValue({ statusCode: 404 });

    const { anime: data, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value).toBeNull();

    expect(error.value).toBeUndefined();

    expect(loading.value).toBe(false);
  });

  it('exposes the error and keeps null on other failures', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const { anime: data, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(data.value).toBeNull();

    expect(loading.value).toBe(false);
  });

  it('rethrows non-404 status codes as errors', async () => {
    apiMock.mockRejectedValue({ statusCode: 500 });

    const { anime: data, error, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(data.value).toBeNull();
  });

  it('uses a distinct cache key per id', async () => {
    apiMock.mockResolvedValue(anime);

    const first = await mountComposable('1');

    await vi.waitFor(() => expect(first.status.value).toBe('success'));

    const second = await mountComposable('2');

    await vi.waitFor(() => expect(second.status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(apiMock).toHaveBeenCalledWith('/animes/1');

    expect(apiMock).toHaveBeenCalledWith('/animes/2');
  });

  it('refetches on refresh', async () => {
    apiMock.mockResolvedValueOnce({ ...anime, title: 'first' });

    apiMock.mockResolvedValueOnce(anime);

    const { anime: data, refresh, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value?.title).toBe('first');

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(data.value).toEqual(anime);
  });
});
