import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppPageCard } from '#components';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatEpisodes, formatType } from '~/utils/anime-utils.ts';

const badges: Badge[] = [
  {
    id: 'animes-score',
    text: formatScore(8.4),
    color: 'neutral',
    icon: 'i-lucide-star',
  },
  {
    id: 'animes-type',
    text: formatType('Tv'),
    color: 'neutral',
    icon: 'i-lucide-tag',
  },
  {
    id: 'animes-episodes',
    text: formatEpisodes(12),
    color: 'neutral',
    icon: 'i-lucide-list-video',
  },
  {
    id: 'animes-status',
    text: formatStatus('FinishedAiring'),
    color: 'neutral',
    icon: 'i-lucide-info',
  },
];

const image = { alt: 'Cover', src: null };

const props = { title: 'title', description: 'description', image, badges };

describe('AppPageCard', () => {
  it('renders title, description and badges', async () => {
    const wrapper = await mountSuspended(AppPageCard, { props });

    expect(wrapper.find('h3').text()).toBe('title');

    expect(wrapper.find('p').text()).toBe('description');

    expect(wrapper.find('[data-testid="animes-score"]').text()).toBe(
      formatScore(8.4),
    );

    expect(wrapper.find('[data-testid="animes-type"]').text()).toBe(
      formatType('Tv'),
    );

    expect(wrapper.find('[data-testid="animes-episodes"]').text()).toBe(
      formatEpisodes(12),
    );

    expect(wrapper.find('[data-testid="animes-status"]').text()).toBe(
      formatStatus('FinishedAiring'),
    );
  });

  it('renders every badge', async () => {
    const wrapper = await mountSuspended(AppPageCard, { props });

    expect(wrapper.findAll('[data-testid^="animes-"]')).toHaveLength(
      badges.length,
    );
  });

  it('renders the image with the given alt', async () => {
    const wrapper = await mountSuspended(AppPageCard, { props });

    expect(wrapper.findComponent({ name: 'AppImage' }).props('alt')).toBe(
      'Cover',
    );
  });
});
