import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { UEmpty, UProgress } from '#components';
import MangaCard from '~/components/MangaCard.vue';
import MangasPage from '~/pages/mangas/index.vue';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const mangasFixture = [
  {
    id: '1bad1470-01d4-4664-80bd-90cced7a6279',
    title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    japaneseTitle: 'かぐや様は告らせたい～天才たちの恋愛頭脳戦～',
    synopsis:
      'At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.',
    chapters: 281,
    volumes: 28,
    status: 'Finished',
    publishedFrom: '2015-05-19',
    publishedTo: '2022-11-02',
    score: 8.88,
    imageSource: '1bad1470-01d4-4664-80bd-90cced7a6279.jpg',
  },
  {
    id: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a',
    title: 'Vagabond',
    japaneseTitle: 'バガボンド',
    synopsis:
      'Takezo, a violent and reckless youth, leaves his village in search of glory. Taking the name Miyamoto Musashi, he walks the path of the sword, and each duel and encounter pushes him to question what true strength really means',
    chapters: 327,
    volumes: 37,
    status: 'Finished',
    publishedFrom: '1988-09-03',
    publishedTo: '2015-05-21',
    score: 9.27,
    imageSource: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a.jpg',
  },
];

const mountedWrappers: Array<{ unmount: () => void }> = [];

const mountPage = async () => {
  const wrapper = await mountSuspended(MangasPage);

  mountedWrappers.push(wrapper);

  return wrapper;
};

describe('MangasPage integration', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData('mangas-all');
  });

  afterEach(() => {
    mountedWrappers.splice(0).forEach((wrapper) => wrapper.unmount());
  });

  it('requests the mangas endpoint', async () => {
    apiMock.mockResolvedValue(mangasFixture);

    await mountPage();

    await vi.waitFor(() => expect(apiMock).toHaveBeenCalledWith('/mangas'));
  });

  it('renders the cards with the data returned by the API', async () => {
    apiMock.mockResolvedValue(mangasFixture);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(2),
    );

    expect(wrapper.find('h2').text()).toBe('Top mangas');

    expect(wrapper.text()).toContain('Vagabond');

    expect(wrapper.text()).toContain('281 chapters');

    expect(wrapper.text()).toContain('28 volumes');

    expect(wrapper.text()).toContain('327 chapters');

    expect(wrapper.text()).toContain('37 volumes');

    expect(wrapper.text()).toContain('Finished');

    expect(wrapper.text()).toContain(
      'Takezo, a violent and reckless youth, leaves his village in search of glory.',
    );

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });

  it('shows the progress bar and no cards while the request is in flight', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const wrapper = await mountPage();

    await vi.waitFor(
      () => expect(wrapper.findComponent(UProgress).exists()).toBe(true),
      { timeout: 2000 },
    );

    expect(wrapper.find('h2').text()).toBe('Top mangas');

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);
  });

  it('shows the error state and no cards when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findComponent(UEmpty).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('Unable to load mangas');

    expect(wrapper.text()).toContain(
      'Something went wrong while fetching the list.',
    );

    expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);
  });

  it('loads the cards after retrying from the error state', async () => {
    apiMock
      .mockRejectedValueOnce(new Error('Network error'))
      .mockResolvedValueOnce(mangasFixture);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findComponent(UEmpty).exists()).toBe(true),
    );

    const retryButton = wrapper
      .findAll('button')
      .find((button) => button.text() === 'Try again');

    expect(retryButton).toBeDefined();

    await retryButton?.trigger('click');

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(2),
    );

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });
});
