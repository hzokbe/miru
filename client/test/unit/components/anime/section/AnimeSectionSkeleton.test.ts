import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AnimeSectionSkeleton, AppPageCardSkeleton } from '#components';

describe('AnimeSectionSkeleton', () => {
  it('renders the page card skeleton', async () => {
    const wrapper = await mountSuspended(AnimeSectionSkeleton);

    expect(wrapper.findComponent(AppPageCardSkeleton).exists()).toBe(true);
  });

  it('passes the placeholder title, description and image', async () => {
    const wrapper = await mountSuspended(AnimeSectionSkeleton);

    const skeleton = wrapper.findComponent(AppPageCardSkeleton);

    expect(skeleton.props('title')).toBe(
      'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    );

    expect(skeleton.props('description')).toContain('Kaguya Shinomiya');

    expect(skeleton.props('image')).toEqual({
      alt: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
      src: null,
    });
  });
});
