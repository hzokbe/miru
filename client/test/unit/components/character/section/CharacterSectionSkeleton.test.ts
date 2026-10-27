import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppPageCardSkeleton, CharacterSectionSkeleton } from '#components';

describe('CharacterSectionSkeleton', () => {
  it('renders the page card skeleton', async () => {
    const wrapper = await mountSuspended(CharacterSectionSkeleton);

    expect(wrapper.findComponent(AppPageCardSkeleton).exists()).toBe(true);
  });

  it('passes the placeholder title, description and image', async () => {
    const wrapper = await mountSuspended(CharacterSectionSkeleton);

    const skeleton = wrapper.findComponent(AppPageCardSkeleton);

    expect(skeleton.props('title')).toBe('Kaguya Shinomiya');

    expect(skeleton.props('description')).toContain('student council');

    expect(skeleton.props('image')).toEqual({
      alt: 'Kaguya Shinomiya',
      src: null,
    });
  });
});
