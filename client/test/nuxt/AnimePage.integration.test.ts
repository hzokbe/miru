import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { UEmpty, UProgress } from '#components';
import AnimePage from '~/pages/animes/[id].vue';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const animeId = '02e9c6be-06e0-479c-9f39-f02c42d02140';

const animeFixture = {
  id: animeId,
  title: 'Steins;Gate',
  japaneseTitle: 'シュタインズ・ゲート',
  synopsis:
    'A self-proclaimed mad scientist finds that his microwave can send messages to the past, with dangerous consequences.',
  episodes: 24,
  type: 'Tv',
  status: 'FinishedAiring',
  airedFrom: '2011-04-06',
  airedTo: '2011-09-14',
  score: 9.07,
  imageSource: '02e9c6be-06e0-479c-9f39-f02c42d02140.jpg',
  genres: [
    {
      id: '4cd47f33-b241-4238-ae56-9100b0385231',
      name: 'Sci-Fi',
      description:
        'Stories built on speculative science and technology, such as time travel, AI and the future.',
    },
    {
      id: '9cb2ae6c-2989-41e9-942b-898d6816cb9e',
      name: 'Drama',
      description:
        'Emotionally driven stories centered on realistic conflicts and character development.',
    },
    {
      id: '61e6c9a7-9876-4335-bc91-b0abbf331e00',
      name: 'Suspense',
      description:
        'Tense stories that keep the audience anxious about what will happen next.',
    },
  ],
};

const mountedWrappers: Array<{ unmount: () => void }> = [];

const mountPage = async () => {
  const wrapper = await mountSuspended(AnimePage, {
    route: `/animes/${animeId}`,
  });

  mountedWrappers.push(wrapper);

  return wrapper;
};

describe('AnimePage integration', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData(`anime-${animeId}`);
  });

  afterEach(() => {
    mountedWrappers.splice(0).forEach((wrapper) => wrapper.unmount());
  });

  it('requests the anime endpoint with the route id', async () => {
    apiMock.mockResolvedValue(animeFixture);

    await mountPage();

    await vi.waitFor(() =>
      expect(apiMock).toHaveBeenCalledWith(`/animes/${animeId}`),
    );
  });

  it('renders the anime returned by the API', async () => {
    apiMock.mockResolvedValue(animeFixture);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(true),
    );

    expect(wrapper.find('h2').text()).toBe('Steins;Gate');

    expect(wrapper.find('h3').text()).toBe('シュタインズ・ゲート');

    expect(wrapper.text()).toContain(animeFixture.synopsis);

    expect(wrapper.text()).toContain('9.07');

    expect(wrapper.text()).toContain('TV');

    expect(wrapper.text()).toContain('24 episodes');

    expect(wrapper.text()).toContain('Finished Airing');

    expect(wrapper.text()).toContain('Apr 06, 2011 - Sep 14, 2011');

    expect(wrapper.text()).toContain('Drama');

    expect(wrapper.text()).toContain('Sci-Fi');

    expect(wrapper.text()).toContain('Suspense');

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });

  it('shows the progress bar and no anime while the request is in flight', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const wrapper = await mountPage();

    await vi.waitFor(
      () => expect(wrapper.findComponent(UProgress).exists()).toBe(true),
      { timeout: 2000 },
    );

    expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(false);
  });

  it('shows the error state and no anime when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findComponent(UEmpty).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('Unable to load anime');

    expect(wrapper.text()).toContain(
      'Something went wrong while fetching the anime details.',
    );

    expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(false);

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);
  });

  it('loads the anime after retrying from the error state', async () => {
    apiMock
      .mockRejectedValueOnce(new Error('Network error'))
      .mockResolvedValueOnce(animeFixture);

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
      expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(true),
    );

    expect(wrapper.find('h2').text()).toBe('Steins;Gate');

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });
});
