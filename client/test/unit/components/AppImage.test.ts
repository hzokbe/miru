import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import AppImage from '~/components/AppImage.vue';

describe('AppImage', () => {
  it('renders a lazy image with alt and src', async () => {
    const wrapper = await mountSuspended(AppImage, {
      props: {
        alt: 'Steins;Gate',
        src: 'd517916a-54ac-478c-94f9-b99359d9f4c9.jpg',
      },
    });

    const image = wrapper.get('img');

    expect(image.attributes('alt')).toBe('Steins;Gate');

    expect(image.attributes('src')).toBe(
      'd517916a-54ac-478c-94f9-b99359d9f4c9.jpg',
    );

    expect(image.attributes('loading')).toBe('lazy');
  });

  it('renders the placeholder when src is null', async () => {
    const wrapper = await mountSuspended(AppImage, {
      props: { alt: 'Steins;Gate', src: null },
    });

    expect(wrapper.find('img').exists()).toBe(false);

    expect(wrapper.element.tagName).toBe('DIV');
  });
});
