import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppCardSkeleton } from '#components';

describe('AppCardSkeleton', () => {
  it('keeps the title invisible to reserve layout space', async () => {
    const wrapper = await mountSuspended(AppCardSkeleton, {
      props: { title: 'title', image: { alt: 'alt', src: null } },
    });

    const title = wrapper.find('h3');

    expect(title.text()).toBe('title');

    expect(title.classes()).toContain('invisible');
  });

  it('renders skeleton placeholders', async () => {
    const wrapper = await mountSuspended(AppCardSkeleton, {
      props: { title: 'title', image: { alt: 'alt', src: null } },
    });

    expect(wrapper.findAllComponents({ name: 'USkeleton' }).length).toBe(3);
  });
});
