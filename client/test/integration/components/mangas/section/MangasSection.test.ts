import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { MangaCard, MangaCardSkeleton, MangasSection } from '#components';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';

const { apiMock, navigateToMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
  navigateToMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('navigateTo', () => navigateToMock);

describe('MangasSection integration', () => {
  beforeEach(() => {
    apiMock.mockReset();

    navigateToMock.mockReset();

    clearNuxtData('mangas-all');
  });

  it('shows skeletons while loading and cards after the response', async () => {
    let resolve!: (value: typeof mangaFixtures) => void;

    apiMock.mockReturnValue(
      new Promise<typeof mangaFixtures>((r) => {
        resolve = r;
      }),
    );

    const wrapper = await mountSuspended(MangasSection);

    expect(wrapper.findAllComponents(MangaCardSkeleton)).toHaveLength(9);

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);

    resolve(mangaFixtures);

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(
        mangaFixtures.length,
      ),
    );

    expect(wrapper.findAllComponents(MangaCardSkeleton)).toHaveLength(0);

    expect(apiMock).toHaveBeenCalledWith('/mangas');
  });

  it('renders titles and formatted badges from the API data', async () => {
    apiMock.mockResolvedValue(mangaFixtures);

    const wrapper = await mountSuspended(MangasSection);

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(
        mangaFixtures.length,
      ),
    );

    const [first] = mangaFixtures;

    const card = wrapper.findAllComponents(MangaCard)[0]!;

    expect(card.text()).toContain(first!.title);

    expect(card.find('[data-testid="manga-score"]').text()).toBe(
      formatScore(first!.score),
    );

    expect(card.find('[data-testid="manga-volumes"]').text()).toBe(
      formatVolumes(first!.volumes),
    );

    expect(card.find('[data-testid="manga-chapters"]').text()).toBe(
      formatChapters(first!.chapters),
    );

    expect(card.find('[data-testid="manga-status"]').text()).toBe(
      formatStatus(first!.status),
    );
  });

  it('navigates to the clicked manga page', async () => {
    apiMock.mockResolvedValue(mangaFixtures);

    const wrapper = await mountSuspended(MangasSection);

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(
        mangaFixtures.length,
      ),
    );

    const target = mangaFixtures[3]!;

    const card = wrapper.findAllComponents(MangaCard)[3]!;

    expect(card.find('a').attributes('href')).toBe(`/mangas/${target.id}`);
  });

  it('shows the empty alert when the API returns no mangas', async () => {
    apiMock.mockResolvedValue([]);

    const wrapper = await mountSuspended(MangasSection);

    await vi.waitFor(() => expect(wrapper.text()).toContain('No manga found'));

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);

    expect(wrapper.findAllComponents(MangaCardSkeleton)).toHaveLength(0);
  });

  it('shows the error alert when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const wrapper = await mountSuspended(MangasSection);

    await vi.waitFor(() =>
      expect(wrapper.text()).toContain(
        'Something went wrong while loading the manga list.',
      ),
    );

    expect(wrapper.text()).not.toContain('No manga found');

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);

    expect(wrapper.findAllComponents(MangaCardSkeleton)).toHaveLength(0);
  });
});
