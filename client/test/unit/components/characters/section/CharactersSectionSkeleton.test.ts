import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { CharacterCardSkeleton, CharactersSectionSkeleton } from '#components';

describe('CharactersSectionSkeleton', () => {
  it('renders section title and nine card skeletons', async () => {
    const wrapper = await mountSuspended(CharactersSectionSkeleton);

    expect(wrapper.text()).toContain('Characters');

    expect(wrapper.findAllComponents(CharacterCardSkeleton)).toHaveLength(9);
  });
});
