import { describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { UColorModeButton, UIcon, UNavigationMenu } from '#components';
import type { NavigationMenuItem } from '@nuxt/ui';
import AppHeader from '~/components/AppHeader.vue';

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const mountAt = (route: string) => mountSuspended(AppHeader, { route });

const getItems = (wrapper: Awaited<ReturnType<typeof mountAt>>) =>
  (
    wrapper.findComponent(UNavigationMenu).props() as unknown as {
      items: NavigationMenuItem[];
    }
  ).items;

const getItem = (items: NavigationMenuItem[], label: string) =>
  items.find((item) => item.label === label);

describe('AppHeader', () => {
  it('renders the app title as a heading', async () => {
    const wrapper = await mountAt('/animes');

    expect(wrapper.find('h1').text()).toBe('Miru');
  });

  it('renders the logo icon', async () => {
    const wrapper = await mountAt('/animes');

    const icons = wrapper.findAllComponents(UIcon);

    expect(icons.map((icon) => icon.props('name'))).toContain('i-lucide-eye');
  });

  it('renders the color mode button', async () => {
    const wrapper = await mountAt('/animes');

    expect(wrapper.findComponent(UColorModeButton).exists()).toBe(true);
  });

  it('renders the Animes and Mangas navigation items', async () => {
    const wrapper = await mountAt('/');

    expect(getItems(wrapper)).toEqual([
      expect.objectContaining({
        label: 'Animes',
        to: '/animes',
        icon: 'i-lucide-clapperboard',
      }),
      expect.objectContaining({
        label: 'Mangas',
        to: '/mangas',
        icon: 'i-lucide-book',
      }),
    ]);
  });

  it('marks only the Animes item as active on the animes page', async () => {
    const wrapper = await mountAt('/animes');

    const items = getItems(wrapper);

    expect(getItem(items, 'Animes')?.active).toBe(true);

    expect(getItem(items, 'Mangas')?.active).toBe(false);
  });

  it('marks only the Mangas item as active on the mangas page', async () => {
    const wrapper = await mountAt('/mangas');

    const items = getItems(wrapper);

    expect(getItem(items, 'Animes')?.active).toBe(false);

    expect(getItem(items, 'Mangas')?.active).toBe(true);
  });

  it('does not mark any item as active on the home page', async () => {
    const wrapper = await mountAt('/');

    expect(getItems(wrapper)).toEqual([
      expect.objectContaining({ active: false }),
      expect.objectContaining({ active: false }),
    ]);
  });

  it('does not mark the Animes item as active on a nested route', async () => {
    const wrapper = await mountAt(
      '/animes/40f3262a-9f1e-4709-a525-2b4b66dc0cac',
    );

    expect(getItems(wrapper)).toEqual([
      expect.objectContaining({ active: false }),
      expect.objectContaining({ active: false }),
    ]);
  });

  it('does not mark any item as active on a nested mangas route', async () => {
    const wrapper = await mountAt(
      '/mangas/2fc7c507-5f91-4a33-9959-2d2ba4a625bb',
    );

    expect(getItems(wrapper)).toEqual([
      expect.objectContaining({ active: false }),
      expect.objectContaining({ active: false }),
    ]);
  });

  it('is fixed and full width on the home page', async () => {
    const wrapper = await mountAt('/');

    const classes = wrapper.find('header').classes();

    expect(classes).toContain('fixed');

    expect(classes).toContain('w-full');
  });

  it('is not fixed on the animes page', async () => {
    const wrapper = await mountAt('/animes');

    const classes = wrapper.find('header').classes();

    expect(classes).not.toContain('fixed');

    expect(classes).not.toContain('w-full');
  });

  it('is not fixed on the mangas page', async () => {
    const wrapper = await mountAt('/mangas');

    const classes = wrapper.find('header').classes();

    expect(classes).not.toContain('fixed');

    expect(classes).not.toContain('w-full');
  });
});
