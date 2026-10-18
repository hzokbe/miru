import { beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { useAnimes } from '~/composables/useAnimes';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

const animesFixture = [
  { id: '02e9c6be-06e0-479c-9f39-f02c42d02140', title: 'Steins;Gate' },
] as unknown as AnimeResponse[];

describe('useAnimes', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData('animes-all');
  });

  it('fetches the animes from the animes endpoint', async () => {
    apiMock.mockResolvedValue(animesFixture);

    const { animes, status } = useAnimes();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledTimes(1);

    expect(apiMock).toHaveBeenCalledWith('/animes');

    expect(animes.value).toEqual(animesFixture);
  });

  it('starts with an empty list while pending', () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { animes, status } = useAnimes();

    expect(animes.value).toEqual([]);

    expect(status.value).toBe('pending');
  });

  it('exposes the error and keeps an empty list when the request fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const { animes, error, status } = useAnimes();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(animes.value).toEqual([]);
  });

  it('fetches again when refresh is called', async () => {
    apiMock.mockResolvedValueOnce([]);

    const { animes, status, refresh } = useAnimes();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    apiMock.mockResolvedValueOnce(animesFixture);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(animes.value).toEqual(animesFixture);
  });
});
