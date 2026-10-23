import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AnimeCardSkeleton, AnimesSectionSkeleton } from '#components';

describe('AnimesSectionSkeleton', () => {
  it('renders section title and nine card skeletons', async () => {
    const wrapper = await mountSuspended(AnimesSectionSkeleton);

    expect(wrapper.text()).toContain('Top animes');

    expect(wrapper.findAllComponents(AnimeCardSkeleton)).toHaveLength(9);
  });
});
