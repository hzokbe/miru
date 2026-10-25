import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppCardSkeleton, CharacterCardSkeleton } from '#components';

describe('CharacterCardSkeleton', () => {
  it('renders the card skeleton', async () => {
    const wrapper = await mountSuspended(CharacterCardSkeleton);

    expect(wrapper.findComponent(AppCardSkeleton).exists()).toBe(true);
  });

  it('passes a placeholder title and an empty image', async () => {
    const wrapper = await mountSuspended(CharacterCardSkeleton);

    const skeleton = wrapper.findComponent(AppCardSkeleton);

    expect(skeleton.props('title')).toBeTruthy();

    expect(skeleton.props('image').src).toBeNull();
  });
});
