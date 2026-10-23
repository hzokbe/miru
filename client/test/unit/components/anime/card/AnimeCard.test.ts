import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AnimeCard } from '#components';

const props = {
  id: 'a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5',
  title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
  type: 'Tv',
  episodes: 12,
  status: 'FinishedAiring',
  score: 8.4,
  imageSource: null,
} as const;

describe('AnimeCard', () => {
  it('renders title and formatted badges', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props });

    expect(wrapper.text()).toContain(
      'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    );

    expect(wrapper.find('[data-testid="anime-score"]').text()).toBe(
      formatScore(props.score),
    );

    expect(wrapper.find('[data-testid="anime-type"]').text()).toBe(
      formatType(props.type),
    );

    expect(wrapper.find('[data-testid="anime-episodes"]').text()).toBe(
      formatEpisodes(props.episodes),
    );

    expect(wrapper.find('[data-testid="anime-status"]').text()).toBe(
      formatStatus(props.status),
    );
  });

  it('renders badges with null score and episodes', async () => {
    const wrapper = await mountSuspended(AnimeCard, {
      props: { ...props, score: null, episodes: null },
    });

    expect(wrapper.find('[data-testid="anime-score"]').text()).toBe(
      formatScore(null),
    );

    expect(wrapper.find('[data-testid="anime-episodes"]').text()).toBe(
      formatEpisodes(null),
    );
  });

  it('links to the anime page', async () => {
    const wrapper = await mountSuspended(AnimeCard, { props });

    expect(wrapper.find('h3 a').attributes('href')).toBe(
      '/animes/a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5',
    );
  });
});
