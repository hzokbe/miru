import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { MangaCard, MangasSection, MangasSectionSkeleton } from '#components';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';

const state = vi.hoisted(() => ({
  mangas: [] as unknown[],
  loading: false,
  error: null as Error | null,
}));

mockNuxtImport('useMangas', async () => {
  const { ref } = await import('vue');

  return () => ({
    mangas: ref(state.mangas),
    loading: ref(state.loading),
    error: ref(state.error),
  });
});

describe('MangasSection', () => {
  beforeEach(() => {
    state.mangas = mangaFixtures;

    state.loading = false;

    state.error = null;
  });

  it('renders the skeleton while loading', async () => {
    state.loading = true;

    const wrapper = await mountSuspended(MangasSection);

    expect(wrapper.findComponent(MangasSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);
  });

  it('renders section title when loaded', async () => {
    const wrapper = await mountSuspended(MangasSection);

    expect(wrapper.findComponent(MangasSectionSkeleton).exists()).toBe(false);

    expect(wrapper.text()).toContain('Top mangas');
  });

  it('renders one card per manga', async () => {
    const wrapper = await mountSuspended(MangasSection);

    const cards = wrapper.findAllComponents(MangaCard);

    expect(cards).toHaveLength(mangaFixtures.length);

    expect(cards.map((card) => card.props('id'))).toEqual(
      mangaFixtures.map((manga) => manga.id),
    );
  });

  it('renders the error alert instead of cards on error', async () => {
    state.error = new Error('request failed');

    const wrapper = await mountSuspended(MangasSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the manga list.',
    );

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);
  });

  it('prioritizes the error alert over the empty alert', async () => {
    state.error = new Error('request failed');

    state.mangas = [];

    const wrapper = await mountSuspended(MangasSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the manga list.',
    );

    expect(wrapper.text()).not.toContain('No manga found');
  });

  it('renders the empty alert when there are no mangas', async () => {
    state.mangas = [];

    const wrapper = await mountSuspended(MangasSection);

    expect(wrapper.text()).toContain('No manga found');

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);
  });
});
