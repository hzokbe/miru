import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { MangaCardSkeleton, MangasSectionSkeleton } from '#components';

describe('MangasSectionSkeleton', () => {
  it('renders section title and nine card skeletons', async () => {
    const wrapper = await mountSuspended(MangasSectionSkeleton);

    expect(wrapper.text()).toContain('Top mangas');

    expect(wrapper.findAllComponents(MangaCardSkeleton)).toHaveLength(9);
  });
});
