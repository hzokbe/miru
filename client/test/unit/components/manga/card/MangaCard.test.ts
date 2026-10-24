import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { MangaCard } from '#components';
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';

const props = {
  id: 'b7d2e4f1-3a6c-4c8b-9e15-7f0a24c6d983',
  title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
  chapters: 281,
  volumes: 28,
  status: 'Finished',
  score: 8.88,
  imageSource: null,
} as const;

describe('MangaCard', () => {
  it('renders title and formatted badges', async () => {
    const wrapper = await mountSuspended(MangaCard, { props });

    expect(wrapper.text()).toContain(
      'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    );

    expect(wrapper.find('[data-testid="manga-score"]').text()).toBe(
      formatScore(props.score),
    );

    expect(wrapper.find('[data-testid="manga-volumes"]').text()).toBe(
      formatVolumes(props.volumes),
    );

    expect(wrapper.find('[data-testid="manga-chapters"]').text()).toBe(
      formatChapters(props.chapters),
    );

    expect(wrapper.find('[data-testid="manga-status"]').text()).toBe(
      formatStatus(props.status),
    );
  });

  it('renders badges with null score, volumes and chapters', async () => {
    const wrapper = await mountSuspended(MangaCard, {
      props: { ...props, score: null, volumes: null, chapters: null },
    });

    expect(wrapper.find('[data-testid="manga-score"]').text()).toBe(
      formatScore(null),
    );

    expect(wrapper.find('[data-testid="manga-volumes"]').text()).toBe(
      formatVolumes(null),
    );

    expect(wrapper.find('[data-testid="manga-chapters"]').text()).toBe(
      formatChapters(null),
    );
  });

  it('links to the manga page', async () => {
    const wrapper = await mountSuspended(MangaCard, { props });

    expect(wrapper.find('h3 a').attributes('href')).toBe(
      '/mangas/b7d2e4f1-3a6c-4c8b-9e15-7f0a24c6d983',
    );
  });
});
