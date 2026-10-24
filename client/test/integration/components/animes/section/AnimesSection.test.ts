import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AnimeSection, AnimeSectionSkeleton, AppPageCard } from '#components';
import { animeFixtures } from '~~/test/fixtures/animes.ts';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatEpisodes, formatType } from '~/utils/anime-utils.ts';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useRoute', () => () => ({
  params: { id: 'a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5' },
}));

describe('AnimesSection integration', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('shows the skeleton while loading and the card after the response', async () => {
    let resolve!: (value: (typeof animeFixtures)[number]) => void;

    apiMock.mockReturnValue(
      new Promise<(typeof animeFixtures)[number]>((r) => {
        resolve = r;
      }),
    );

    const wrapper = await mountSuspended(AnimeSection);

    expect(wrapper.findComponent(AnimeSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    resolve(animeFixtures[0]!);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.findComponent(AnimeSectionSkeleton).exists()).toBe(false);

    expect(apiMock).toHaveBeenCalledWith(
      '/animes/a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5',
    );
  });

  it('renders title, synopsis and formatted badges from the API data', async () => {
    const anime = animeFixtures[0]!;

    apiMock.mockResolvedValue(anime);

    const wrapper = await mountSuspended(AnimeSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain(anime.title);

    expect(wrapper.text()).toContain(anime.synopsis ?? 'No synopsis');

    expect(wrapper.find('[data-testid="anime-score"]').text()).toBe(
      formatScore(anime.score),
    );

    expect(wrapper.find('[data-testid="anime-episodes"]').text()).toBe(
      formatEpisodes(anime.episodes),
    );

    expect(wrapper.find('[data-testid="anime-status"]').text()).toBe(
      formatStatus(anime.status),
    );

    expect(wrapper.find('[data-testid="anime-type"]').text()).toBe(
      formatType(anime.type),
    );

    expect(wrapper.find('[data-testid="anime-aired"]').exists()).toBe(true);
  });

  it('renders the fallback description when the anime has no synopsis', async () => {
    apiMock.mockResolvedValue({ ...animeFixtures[0]!, synopsis: null });

    const wrapper = await mountSuspended(AnimeSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('No synopsis');
  });

  it('shows the not found alert when the API returns no anime', async () => {
    apiMock.mockResolvedValue(null);

    const wrapper = await mountSuspended(AnimeSection);

    await vi.waitFor(() => expect(wrapper.text()).toContain('Anime not found'));

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    expect(wrapper.findComponent(AnimeSectionSkeleton).exists()).toBe(false);
  });

  it('shows the error alert when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const wrapper = await mountSuspended(AnimeSection);

    await vi.waitFor(() =>
      expect(wrapper.text()).toContain(
        'Something went wrong while loading the anime details. Please try again.',
      ),
    );

    expect(wrapper.text()).not.toContain('Anime not found');

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    expect(wrapper.findComponent(AnimeSectionSkeleton).exists()).toBe(false);
  });
});
