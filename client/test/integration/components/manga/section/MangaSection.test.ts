import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AppPageCard, MangaSection, MangaSectionSkeleton } from '#components';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useRoute', () => () => ({ params: { id: '1' } }));

describe('MangaSection integration', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('shows the skeleton while loading and the card after the response', async () => {
    let resolve!: (value: (typeof mangaFixtures)[number]) => void;

    apiMock.mockReturnValue(
      new Promise<(typeof mangaFixtures)[number]>((r) => {
        resolve = r;
      }),
    );

    const wrapper = await mountSuspended(MangaSection);

    expect(wrapper.findComponent(MangaSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    resolve(mangaFixtures[0]!);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.findComponent(MangaSectionSkeleton).exists()).toBe(false);

    expect(apiMock).toHaveBeenCalledWith('/mangas/1');
  });

  it('renders title, synopsis and formatted badges from the API data', async () => {
    const manga = mangaFixtures[0]!;

    apiMock.mockResolvedValue(manga);

    const wrapper = await mountSuspended(MangaSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain(manga.title);

    expect(wrapper.text()).toContain(manga.synopsis ?? 'No synopsis');

    expect(wrapper.find('[data-testid="manga-score"]').text()).toBe(
      formatScore(manga.score),
    );

    expect(wrapper.find('[data-testid="manga-chapters"]').text()).toBe(
      formatChapters(manga.chapters),
    );

    expect(wrapper.find('[data-testid="manga-volumes"]').text()).toBe(
      formatVolumes(manga.volumes),
    );

    expect(wrapper.find('[data-testid="manga-status"]').text()).toBe(
      formatStatus(manga.status),
    );

    expect(wrapper.find('[data-testid="manga-published"]').exists()).toBe(true);
  });

  it('renders the fallback description when the manga has no synopsis', async () => {
    apiMock.mockResolvedValue({ ...mangaFixtures[0]!, synopsis: null });

    const wrapper = await mountSuspended(MangaSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('No synopsis');
  });

  it('shows the not found alert when the API responds 404', async () => {
    apiMock.mockRejectedValue({ statusCode: 404 });

    const wrapper = await mountSuspended(MangaSection);

    await vi.waitFor(() => expect(wrapper.text()).toContain('Manga not found'));

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    expect(wrapper.findComponent(MangaSectionSkeleton).exists()).toBe(false);
  });

  it('shows the error alert when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const wrapper = await mountSuspended(MangaSection);

    await vi.waitFor(() =>
      expect(wrapper.text()).toContain(
        'Something went wrong while loading the manga details. Please try again.',
      ),
    );

    expect(wrapper.text()).not.toContain('Manga not found');

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    expect(wrapper.findComponent(MangaSectionSkeleton).exists()).toBe(false);
  });
});
