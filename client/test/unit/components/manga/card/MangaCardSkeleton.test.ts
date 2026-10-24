import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppCardSkeleton, MangaCardSkeleton } from '#components';

describe('MangaCardSkeleton', () => {
  it('renders an AppCardSkeleton with placeholder data', async () => {
    const wrapper = await mountSuspended(MangaCardSkeleton);

    const skeleton = wrapper.findComponent(AppCardSkeleton);

    expect(skeleton.exists()).toBe(true);

    expect(skeleton.props('title')).toBe(
      'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    );

    expect(skeleton.props('image')).toEqual({
      alt: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
      src: null,
    });
  });
});
