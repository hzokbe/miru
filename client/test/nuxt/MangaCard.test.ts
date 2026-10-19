import { describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import AppImage from '~/components/AppImage.vue';
import MangaCard from '~/components/MangaCard.vue';
import type { MangaStatus } from '~/types/manga-response.ts';

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const baseProps = {
  id: '3a3d026e-840c-4fdc-a0b8-c1f2230e6c90',
  title: 'Sousou no Frieren',
  synopsis:
    "After a hero's party defeats the Demon King, the long-lived elf mage Frieren outlives her old companions. Realizing she never truly got to know them, she sets out on a new journey with a young apprentice to revisit the places they once shared.",
  chapters: 147,
  volumes: 15,
  status: 'OnHiatus' as MangaStatus,
  score: 8.87,
  imageSource: '3a3d026e-840c-4fdc-a0b8-c1f2230e6c90.jpg',
};

describe('MangaCard', () => {
  it('renders the title', async () => {
    const wrapper = await mountSuspended(MangaCard, { props: baseProps });

    expect(wrapper.find('h3').text()).toBe('Sousou no Frieren');
  });

  it('links the title and the image to the manga page', async () => {
    const wrapper = await mountSuspended(MangaCard, { props: baseProps });

    const links = wrapper.findAll('a');

    expect(links).toHaveLength(2);

    links.forEach((link) => {
      expect(link.attributes('href')).toBe(
        '/mangas/3a3d026e-840c-4fdc-a0b8-c1f2230e6c90',
      );
    });
  });

  it('renders the synopsis when provided', async () => {
    const wrapper = await mountSuspended(MangaCard, { props: baseProps });

    expect(wrapper.text()).toContain(baseProps.synopsis);

    expect(wrapper.text()).not.toContain('No synopsis');
  });

  it('renders a fallback when the synopsis is null', async () => {
    const wrapper = await mountSuspended(MangaCard, {
      props: { ...baseProps, synopsis: null },
    });

    expect(wrapper.text()).toContain('No synopsis');
  });

  it('renders the score, chapters, volumes and status', async () => {
    const wrapper = await mountSuspended(MangaCard, {
      props: { ...baseProps },
    });

    expect(wrapper.get('[data-testid="manga-score"]').text()).toBe('8.87');

    expect(wrapper.get('[data-testid="manga-chapters"]').text()).toBe(
      '0 chapters',
    );

    expect(wrapper.get('[data-testid="manga-volumes"]').text()).toBe(
      '0 volumes',
    );

    expect(wrapper.get('[data-testid="manga-status"]').text()).toBe(
      'On Hiatus',
    );
  });

  it('renders fallbacks when score is null', async () => {
    const wrapper = await mountSuspended(MangaCard, {
      props: { ...baseProps, score: null },
    });

    expect(wrapper.get('[data-testid="manga-score"]').text()).toBe('0');
  });

  it('formats the status by splitting camel case words', async () => {
    const wrapper = await mountSuspended(MangaCard, { props: baseProps });

    expect(wrapper.text()).toContain('On Hiatus');

    expect(wrapper.text()).not.toContain('OnHiatus');
  });

  it('keeps single word statuses unchanged', async () => {
    const wrapper = await mountSuspended(MangaCard, {
      props: { ...baseProps, status: 'Finished' as MangaStatus },
    });

    expect(wrapper.get('[data-testid="manga-status"]').text()).toBe('Finished');
  });

  it('passes the title and image source to AppImage', async () => {
    const wrapper = await mountSuspended(MangaCard, { props: baseProps });

    const image = wrapper.findComponent(AppImage);

    expect(image.exists()).toBe(true);

    expect(image.props('alt')).toBe('Sousou no Frieren');

    expect(image.props('src')).toBe('3a3d026e-840c-4fdc-a0b8-c1f2230e6c90.jpg');
  });

  it('passes a null source to AppImage', async () => {
    const wrapper = await mountSuspended(MangaCard, {
      props: { ...baseProps, imageSource: null },
    });

    expect(wrapper.findComponent(AppImage).props('src')).toBeNull();
  });
});
