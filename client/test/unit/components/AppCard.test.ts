import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppCard } from '#components';

const badges: Badge[] = [
  {
    id: 'anime-score',
    text: formatScore(8.4),
    color: 'neutral',
    icon: 'i-lucide-star',
  },
  {
    id: 'anime-type',
    text: formatType('Tv'),
    color: 'neutral',
    icon: 'i-lucide-tag',
  },
  {
    id: 'anime-episodes',
    text: formatEpisodes(12),
    color: 'neutral',
    icon: 'i-lucide-list-video',
  },
  {
    id: 'anime-status',
    text: formatStatus('FinishedAiring'),
    color: 'neutral',
    icon: 'i-lucide-info',
  },
];

const image = { alt: 'Cover', src: null };

const props = {
  title: 'title',
  image,
  badges,
  to: '/animes/a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5',
};

describe('AppCard', () => {
  it('renders title and badges', async () => {
    const wrapper = await mountSuspended(AppCard, { props });

    expect(wrapper.text()).toContain('title');

    expect(wrapper.find('[data-testid="anime-score"]').text()).toBe(
      formatScore(8.4),
    );

    expect(wrapper.find('[data-testid="anime-type"]').text()).toBe(
      formatType('Tv'),
    );

    expect(wrapper.find('[data-testid="anime-episodes"]').text()).toBe(
      formatEpisodes(12),
    );

    expect(wrapper.find('[data-testid="anime-status"]').text()).toBe(
      formatStatus('FinishedAiring'),
    );
  });

  it('sets the title attribute', async () => {
    const wrapper = await mountSuspended(AppCard, { props });

    expect(wrapper.find('h3').attributes('title')).toBe('title');
  });

  it('renders the title as a link to the given route', async () => {
    const wrapper = await mountSuspended(AppCard, { props });

    const link = wrapper.find('h3 a');

    expect(link.exists()).toBe(true);

    expect(link.attributes('href')).toBe(
      '/animes/a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5',
    );

    expect(link.text()).toBe('title');
  });
});
