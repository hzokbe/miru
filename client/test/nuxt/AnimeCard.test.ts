import { describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import AnimeCard from '~/components/AnimeCard.vue';
import AppImage from '~/components/AppImage.vue';

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const baseProps = {
  id: '02e9c6be-06e0-479c-9f39-f02c42d02140',
  title: 'Steins;Gate',
  synopsis:
    'A self-proclaimed mad scientist finds that his microwave can send messages to the past, with dangerous consequences.',
  episodes: 24,
  status: 'FinishedAiring' as AnimeStatus,
  score: 9.07,
  imageSource: '02e9c6be-06e0-479c-9f39-f02c42d02140.jpg',
};

describe('AnimeCard', () => {
  it('renders the title', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props: baseProps });

    expect(wrapper.find('h3').text()).toBe('Steins;Gate');
  });

  it('links the title and the image to the anime page', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props: baseProps });

    const links = wrapper.findAll('a');

    expect(links).toHaveLength(2);

    links.forEach((link) => {
      expect(link.attributes('href')).toBe(
        '/animes/02e9c6be-06e0-479c-9f39-f02c42d02140',
      );
    });
  });

  it('renders the synopsis when provided', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props: baseProps });

    expect(wrapper.text()).toContain(baseProps.synopsis);

    expect(wrapper.text()).not.toContain('No synopsis');
  });

  it('renders a fallback when the synopsis is null', async () => {
    const wrapper = await mountSuspended(AnimeCard, {
      props: { ...baseProps, synopsis: null },
    });

    expect(wrapper.text()).toContain('No synopsis');
  });

  it('renders the score and the episode count', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props: baseProps });

    expect(wrapper.get('[data-testid="anime-score"]').text()).toBe('9.07');

    expect(wrapper.get('[data-testid="anime-episodes"]').text()).toBe(
      '24 episodes',
    );

    expect(wrapper.get('[data-testid="anime-status"]').text()).toBe(
      'Finished Airing',
    );
  });

  it('renders fallbacks when score and episodes are null', async () => {
    const wrapper = await mountSuspended(AnimeCard, {
      props: { ...baseProps, score: null, episodes: null },
    });

    expect(wrapper.get('[data-testid="anime-score"]').text()).toBe('0');

    expect(wrapper.get('[data-testid="anime-episodes"]').text()).toBe(
      '0 episodes',
    );
  });

  it('formats the status by splitting camel case words', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props: baseProps });

    expect(wrapper.text()).toContain('Finished Airing');

    expect(wrapper.text()).not.toContain('FinishedAiring');
  });

  it('passes the title and image source to AppImage', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props: baseProps });

    const image = wrapper.findComponent(AppImage);

    expect(image.exists()).toBe(true);

    expect(image.props('alt')).toBe('Steins;Gate');

    expect(image.props('src')).toBe('02e9c6be-06e0-479c-9f39-f02c42d02140.jpg');
  });

  it('passes a null source to AppImage', async () => {
    const wrapper = await mountSuspended(AnimeCard, {
      props: { ...baseProps, imageSource: null },
    });

    expect(wrapper.findComponent(AppImage).props('src')).toBeNull();
  });
});
