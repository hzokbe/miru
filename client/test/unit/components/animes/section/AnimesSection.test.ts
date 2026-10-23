import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AnimeCard, AnimesSection, AnimesSectionSkeleton } from '#components';
import { animeFixtures } from '~~/test/fixtures/animes.ts';

const state = vi.hoisted(() => ({
  animes: [] as unknown[],
  loading: false,
  error: null as Error | null,
}));

mockNuxtImport('useAnimes', async () => {
  const { ref } = await import('vue');

  return () => ({
    animes: ref(state.animes),
    loading: ref(state.loading),
    error: ref(state.error),
  });
});

describe('AnimesSection', () => {
  beforeEach(() => {
    state.animes = animeFixtures;

    state.loading = false;

    state.error = null;
  });

  it('renders the skeleton while loading', async () => {
    state.loading = true;

    const wrapper = await mountSuspended(AnimesSection);

    expect(wrapper.findComponent(AnimesSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);
  });

  it('renders section title when loaded', async () => {
    const wrapper = await mountSuspended(AnimesSection);

    expect(wrapper.findComponent(AnimesSectionSkeleton).exists()).toBe(false);

    expect(wrapper.text()).toContain('Top animes');
  });

  it('renders one card per anime', async () => {
    const wrapper = await mountSuspended(AnimesSection);

    const cards = wrapper.findAllComponents(AnimeCard);

    expect(cards).toHaveLength(animeFixtures.length);

    expect(cards.map((card) => card.props('id'))).toEqual(
      animeFixtures.map((anime) => anime.id),
    );
  });

  it('renders the error alert instead of cards on error', async () => {
    state.error = new Error('request failed');

    const wrapper = await mountSuspended(AnimesSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the anime list.',
    );

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);
  });

  it('prioritizes the error alert over the empty alert', async () => {
    state.error = new Error('request failed');

    state.animes = [];

    const wrapper = await mountSuspended(AnimesSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the anime list.',
    );

    expect(wrapper.text()).not.toContain('No anime found');
  });

  it('renders the empty alert when there are no animes', async () => {
    state.animes = [];

    const wrapper = await mountSuspended(AnimesSection);

    expect(wrapper.text()).toContain('No anime found');

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);
  });
});
