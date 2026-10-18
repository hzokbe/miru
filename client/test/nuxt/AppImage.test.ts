import { describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import AppImage from '~/components/AppImage.vue';

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const baseProps = {
  alt: 'Steins;Gate',
  src: '02e9c6be-06e0-479c-9f39-f02c42d02140.jpg',
};

describe('AppImage', () => {
  it('renders an img when src is provided', async () => {
    const wrapper = await mountSuspended(AppImage, { props: baseProps });

    const image = wrapper.find('img');

    expect(image.exists()).toBe(true);

    expect(image.attributes('src')).toBe(
      '02e9c6be-06e0-479c-9f39-f02c42d02140.jpg',
    );

    expect(image.attributes('alt')).toBe('Steins;Gate');
  });

  it('loads the image lazily', async () => {
    const wrapper = await mountSuspended(AppImage, { props: baseProps });

    expect(wrapper.find('img').attributes('loading')).toBe('lazy');
  });

  it('does not render the fallback when src is provided', async () => {
    const wrapper = await mountSuspended(AppImage, { props: baseProps });

    expect(wrapper.findComponent({ name: 'UIcon' }).exists()).toBe(false);
  });

  it('renders the fallback when src is null', async () => {
    const wrapper = await mountSuspended(AppImage, {
      props: { ...baseProps, src: null },
    });

    expect(wrapper.find('img').exists()).toBe(false);

    expect(wrapper.findComponent({ name: 'UIcon' }).exists()).toBe(true);
  });

  it('renders the fallback when src is an empty string', async () => {
    const wrapper = await mountSuspended(AppImage, {
      props: { ...baseProps, src: '' },
    });

    expect(wrapper.find('img').exists()).toBe(false);

    expect(wrapper.findComponent({ name: 'UIcon' }).exists()).toBe(true);
  });

  it('passes the image off icon to the fallback', async () => {
    const wrapper = await mountSuspended(AppImage, {
      props: { ...baseProps, src: null },
    });

    expect(wrapper.findComponent({ name: 'UIcon' }).props('name')).toBe(
      'i-lucide-image-off',
    );
  });
});
