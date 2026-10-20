import { beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { useManga } from '~/composables/useManga';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

const mangaId = 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a';

const mangaFixture = {
  id: mangaId,
  title: 'Vagabond',
} as unknown as MangaResponse;

describe('useManga', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData(`manga-${mangaId}`);
  });

  it('fetches the manga from the manga endpoint', async () => {
    apiMock.mockResolvedValue(mangaFixture);

    const { manga, status } = useManga(mangaId);

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledTimes(1);

    expect(apiMock).toHaveBeenCalledWith(`/mangas/${mangaId}`);

    expect(manga.value).toEqual(mangaFixture);
  });

  it('starts with a null manga while pending', () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { manga, status } = useManga(mangaId);

    expect(manga.value).toBeNull();

    expect(status.value).toBe('pending');
  });

  it('exposes the error and keeps a null manga when the request fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const { manga, error, status } = useManga(mangaId);

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(manga.value).toBeNull();
  });

  it('fetches again when refresh is called', async () => {
    apiMock.mockResolvedValueOnce(null);

    const { manga, status, refresh } = useManga(mangaId);

    await vi.waitFor(() => expect(status.value).toBe('success'));

    apiMock.mockResolvedValueOnce(mangaFixture);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(manga.value).toEqual(mangaFixture);
  });

  it('keeps separate data for different ids', async () => {
    const otherId = '1bad1470-01d4-4664-80bd-90cced7a6279';

    const otherFixture = {
      id: otherId,
      title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    } as unknown as MangaResponse;

    clearNuxtData(`manga-${otherId}`);

    apiMock.mockResolvedValueOnce(mangaFixture);

    apiMock.mockResolvedValueOnce(otherFixture);

    const first = useManga(mangaId);

    const second = useManga(otherId);

    await vi.waitFor(() => expect(first.status.value).toBe('success'));

    await vi.waitFor(() => expect(second.status.value).toBe('success'));

    expect(first.manga.value).toEqual(mangaFixture);

    expect(second.manga.value).toEqual(otherFixture);

    expect(apiMock).toHaveBeenCalledWith(`/mangas/${mangaId}`);

    expect(apiMock).toHaveBeenCalledWith(`/mangas/${otherId}`);
  });
});
