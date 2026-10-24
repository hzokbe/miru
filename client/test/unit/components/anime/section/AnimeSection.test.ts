import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AnimeSection, AnimeSectionSkeleton, AppPageCard } from '#components';
import { animeFixtures } from '~~/test/fixtures/animes.ts';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatEpisodes, formatType } from '~/utils/anime-utils.ts';

const state = vi.hoisted(() => ({
  anime: null as unknown,
  loading: false,
  error: null as Error | null,
}));

mockNuxtImport('useRoute', () => () => ({ params: { id: '1' } }));

mockNuxtImport('useAnime', async () => {
  const { ref } = await import('vue');

  return () => ({
    anime: ref(state.anime),
    loading: ref(state.loading),
    error: ref(state.error),
  });
});

describe('AnimeSection', () => {
  beforeEach(() => {
    state.anime = animeFixtures[0];

    state.loading = false;

    state.error = null;
  });

  it('renders the skeleton while loading', async () => {
    state.loading = true;

    const wrapper = await mountSuspended(AnimeSection);

    expect(wrapper.findComponent(AnimeSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });

  it('renders the page card with animes data', async () => {
    const wrapper = await mountSuspended(AnimeSection);

    const anime = animeFixtures[0]!;

    const card = wrapper.findComponent(AppPageCard);

    expect(wrapper.findComponent(AnimeSectionSkeleton).exists()).toBe(false);

    expect(card.props('title')).toBe(anime.title);

    expect(card.props('description')).toBe(anime.synopsis ?? 'No synopsis');

    expect(card.props('image')).toEqual({
      alt: anime.title,
      src: anime.imageSource,
    });
  });

  it('renders formatted badges', async () => {
    const wrapper = await mountSuspended(AnimeSection);

    const anime = animeFixtures[0]!;

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

  it('renders the fallback description when there is no synopsis', async () => {
    state.anime = { ...animeFixtures[0]!, synopsis: null };

    const wrapper = await mountSuspended(AnimeSection);

    expect(wrapper.findComponent(AppPageCard).props('description')).toBe(
      'No synopsis',
    );
  });

  it('renders the error alert instead of the card on error', async () => {
    state.error = new Error('request failed');

    const wrapper = await mountSuspended(AnimeSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the anime details. Please try again.',
    );

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });

  it('prioritizes the error alert over the not found alert', async () => {
    state.error = new Error('request failed');

    state.anime = null;

    const wrapper = await mountSuspended(AnimeSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the anime details. Please try again.',
    );

    expect(wrapper.text()).not.toContain('Anime not found');
  });

  it('renders the not found alert when there is no animes', async () => {
    state.anime = null;

    const wrapper = await mountSuspended(AnimeSection);

    expect(wrapper.text()).toContain('Anime not found');

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });
});
