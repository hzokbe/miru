import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { MangaSection, MangaSectionSkeleton, AppPageCard } from '#components';
import { mangaFixtures } from '~~/test/fixtures/mangas.ts';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';

const state = vi.hoisted(() => ({
  manga: null as unknown,
  loading: false,
  error: null as Error | null,
}));

mockNuxtImport('useRoute', () => () => ({ params: { id: '1' } }));

mockNuxtImport('useManga', async () => {
  const { ref } = await import('vue');

  return () => ({
    manga: ref(state.manga),
    loading: ref(state.loading),
    error: ref(state.error),
  });
});

describe('MangaSection', () => {
  beforeEach(() => {
    state.manga = mangaFixtures[0];

    state.loading = false;

    state.error = null;
  });

  it('renders the skeleton while loading', async () => {
    state.loading = true;

    const wrapper = await mountSuspended(MangaSection);

    expect(wrapper.findComponent(MangaSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });

  it('renders the page card with manga data', async () => {
    const wrapper = await mountSuspended(MangaSection);

    const manga = mangaFixtures[0]!;

    const card = wrapper.findComponent(AppPageCard);

    expect(wrapper.findComponent(MangaSectionSkeleton).exists()).toBe(false);

    expect(card.props('title')).toBe(manga.title);

    expect(card.props('description')).toBe(manga.synopsis ?? 'No synopsis');

    expect(card.props('image')).toEqual({
      alt: manga.title,
      src: manga.imageSource,
    });
  });

  it('renders formatted badges', async () => {
    const wrapper = await mountSuspended(MangaSection);

    const manga = mangaFixtures[0]!;

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

  it('renders the fallback description when there is no synopsis', async () => {
    state.manga = { ...mangaFixtures[0]!, synopsis: null };

    const wrapper = await mountSuspended(MangaSection);

    expect(wrapper.findComponent(AppPageCard).props('description')).toBe(
      'No synopsis',
    );
  });

  it('renders the error alert instead of the card on error', async () => {
    state.error = new Error('request failed');

    const wrapper = await mountSuspended(MangaSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the manga details. Please try again.',
    );

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });

  it('prioritizes the error alert over the not found alert', async () => {
    state.error = new Error('request failed');

    state.manga = null;

    const wrapper = await mountSuspended(MangaSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the manga details. Please try again.',
    );

    expect(wrapper.text()).not.toContain('Manga not found');
  });

  it('renders the not found alert when there is no manga', async () => {
    state.manga = null;

    const wrapper = await mountSuspended(MangaSection);

    expect(wrapper.text()).toContain('Manga not found');

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });
});
