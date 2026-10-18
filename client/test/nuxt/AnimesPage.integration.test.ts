import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { UEmpty, UProgress } from '#components';
import AnimeCard from '~/components/AnimeCard.vue';
import AnimesPage from '~/pages/animes/index.vue';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const animesFixture = [
  {
    id: '02e9c6be-06e0-479c-9f39-f02c42d02140',
    title: 'Steins;Gate',
    synopsis:
      'A self-proclaimed mad scientist finds that his microwave can send messages to the past, with dangerous consequences.',
    episodes: 24,
    status: 'FinishedAiring',
    score: 9.07,
    imageSource: '02e9c6be-06e0-479c-9f39-f02c42d02140.jpg',
  },
  {
    id: '125e0fcc-0962-4b0b-be9c-95b09ad623a3',
    title: 'Sousou no Frieren',
    synopsis:
      'Decades after defeating the Demon King, the elf Frieren realizes how much she overlooked her companions and sets out to understand humans.',
    episodes: 28,
    status: 'FinishedAiring',
    score: 9.24,
    imageSource: '125e0fcc-0962-4b0b-be9c-95b09ad623a3.jpg',
  },
];

const mountedWrappers: Array<{ unmount: () => void }> = [];

const mountPage = async () => {
  const wrapper = await mountSuspended(AnimesPage);

  mountedWrappers.push(wrapper);

  return wrapper;
};

describe('AnimesPage integration', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData('animes-all');
  });

  afterEach(() => {
    mountedWrappers.splice(0).forEach((wrapper) => wrapper.unmount());
  });

  it('requests the animes endpoint', async () => {
    apiMock.mockResolvedValue(animesFixture);

    await mountPage();

    await vi.waitFor(() => expect(apiMock).toHaveBeenCalledWith('/animes'));
  });

  it('renders the cards with the data returned by the API', async () => {
    apiMock.mockResolvedValue(animesFixture);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(2),
    );

    expect(wrapper.find('h2').text()).toBe('Top animes');

    expect(wrapper.text()).toContain('Steins;Gate');

    expect(wrapper.text()).toContain('9.07');

    expect(wrapper.text()).toContain('24 episodes');

    expect(wrapper.text()).toContain('Finished Airing');

    expect(wrapper.text()).toContain(
      'A self-proclaimed mad scientist finds that his microwave can send messages to the past, with dangerous consequences.',
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

    expect(wrapper.find('h2').text()).toBe('Top animes');

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);
  });

  it('shows the error state and no cards when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findComponent(UEmpty).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('Unable to load animes');

    expect(wrapper.text()).toContain(
      'Something went wrong while fetching the list.',
    );

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);
  });

  it('loads the cards after retrying from the error state', async () => {
    apiMock
      .mockRejectedValueOnce(new Error('Network error'))
      .mockResolvedValueOnce(animesFixture);

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
      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(2),
    );

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });
});
