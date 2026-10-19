import { beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { useAnime } from '~/composables/useAnime';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

const animeId = '02e9c6be-06e0-479c-9f39-f02c42d02140';

const animeFixture = {
  id: animeId,
  title: 'Steins;Gate',
} as unknown as AnimeResponse;

describe('useAnime', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData(`anime-${animeId}`);
  });

  it('fetches the anime from the anime endpoint', async () => {
    apiMock.mockResolvedValue(animeFixture);

    const { anime, status } = useAnime(animeId);

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledTimes(1);

    expect(apiMock).toHaveBeenCalledWith(`/animes/${animeId}`);

    expect(anime.value).toEqual(animeFixture);
  });

  it('starts with a null anime while pending', () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { anime, status } = useAnime(animeId);

    expect(anime.value).toBeNull();

    expect(status.value).toBe('pending');
  });

  it('exposes the error and keeps a null anime when the request fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const { anime, error, status } = useAnime(animeId);

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(anime.value).toBeNull();
  });

  it('fetches again when refresh is called', async () => {
    apiMock.mockResolvedValueOnce(null);

    const { anime, status, refresh } = useAnime(animeId);

    await vi.waitFor(() => expect(status.value).toBe('success'));

    apiMock.mockResolvedValueOnce(animeFixture);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(anime.value).toEqual(animeFixture);
  });

  it('keeps separate data for different ids', async () => {
    const otherId = '125e0fcc-0962-4b0b-be9c-95b09ad623a3';

    const otherFixture = {
      id: otherId,
      title: 'Sousou no Frieren',
    } as unknown as AnimeResponse;

    clearNuxtData(`anime-${otherId}`);

    apiMock.mockResolvedValueOnce(animeFixture);

    apiMock.mockResolvedValueOnce(otherFixture);

    const first = useAnime(animeId);

    const second = useAnime(otherId);

    await vi.waitFor(() => expect(first.status.value).toBe('success'));

    await vi.waitFor(() => expect(second.status.value).toBe('success'));

    expect(first.anime.value).toEqual(animeFixture);

    expect(second.anime.value).toEqual(otherFixture);

    expect(apiMock).toHaveBeenCalledWith(`/animes/${animeId}`);

    expect(apiMock).toHaveBeenCalledWith(`/animes/${otherId}`);
  });
});
