import { beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { useMangas } from '~/composables/useMangas';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

const mangasFixture = [
  { id: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a', title: 'Vagabond' },
] as unknown as MangaResponse[];

describe('useMangas', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData('mangas-all');
  });

  it('fetches the mangas from the mangas endpoint', async () => {
    apiMock.mockResolvedValue(mangasFixture);

    const { mangas, status } = useMangas();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledTimes(1);

    expect(apiMock).toHaveBeenCalledWith('/mangas');

    expect(mangas.value).toEqual(mangasFixture);
  });

  it('starts with an empty list while pending', () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { mangas, status } = useMangas();

    expect(mangas.value).toEqual([]);

    expect(status.value).toBe('pending');
  });

  it('exposes the error and keeps an empty list when the request fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const { mangas, error, status } = useMangas();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(mangas.value).toEqual([]);
  });

  it('fetches again when refresh is called', async () => {
    apiMock.mockResolvedValueOnce([]);

    const { mangas, status, refresh } = useMangas();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    apiMock.mockResolvedValueOnce(mangasFixture);

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(mangas.value).toEqual(mangasFixture);
  });
});
