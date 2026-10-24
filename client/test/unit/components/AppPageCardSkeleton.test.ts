import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppPageCardSkeleton } from '#components';

const props = {
  title: 'title',
  description: 'description',
  image: { alt: 'alt', src: null },
};

describe('AppPageCardSkeleton', () => {
  it('keeps the title invisible to reserve layout space', async () => {
    const wrapper = await mountSuspended(AppPageCardSkeleton, { props });

    const title = wrapper.find('h3');

    expect(title.text()).toBe('title');

    expect(title.classes()).toContain('invisible');
  });

  it('keeps the description invisible to reserve layout space', async () => {
    const wrapper = await mountSuspended(AppPageCardSkeleton, { props });

    const description = wrapper.find('p');

    expect(description.text()).toBe('description');

    expect(description.classes()).toContain('invisible');
  });

  it('keeps the image invisible to reserve layout space', async () => {
    const wrapper = await mountSuspended(AppPageCardSkeleton, { props });

    expect(wrapper.findComponent({ name: 'AppImage' }).classes()).toContain(
      'invisible',
    );
  });

  it('renders skeleton placeholders', async () => {
    const wrapper = await mountSuspended(AppPageCardSkeleton, { props });

    expect(wrapper.findAllComponents({ name: 'USkeleton' }).length).toBe(4);
  });
});
