import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { UEmpty, UProgress } from '#components';
import MangaPage from '~/pages/mangas/[id].vue';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const mangasFixture = [
  {
    id: '1bad1470-01d4-4664-80bd-90cced7a6279',
    title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    japaneseTitle: null,
    synopsis:
      'At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.',
    chapters: 281,
    volumes: 28,
    status: 'Finished',
    publishedFrom: '2015-05-19',
    publishedTo: '2022-11-02',
    score: 8.88,
    imageSource: '1bad1470-01d4-4664-80bd-90cced7a6279.jpg',
    genres: [
      {
        id: '0fec3a53-a8d9-4b06-974b-4fb0211f9556',
        name: 'Comedy',
        description:
          'Humor-focused stories meant to entertain through jokes, absurd situations and witty banter.',
      },
      {
        id: '76918d56-1d6b-4603-b2c4-9cf8e4f2625e',
        name: 'Romance',
        description:
          'Stories centered on love, attraction and the development of romantic relationships.',
      },
    ],
  },
  {
    id: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a',
    title: 'Vagabond',
    japaneseTitle: null,
    synopsis:
      'Takezo, a violent and reckless youth, leaves his village in search of glory. Taking the name Miyamoto Musashi, he walks the path of the sword, and each duel and encounter pushes him to question what true strength really means',
    chapters: 327,
    volumes: 37,
    status: 'Finished',
    publishedFrom: '1988-09-03',
    publishedTo: '2015-05-21',
    score: 9.27,
    imageSource: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a.jpg',
    genres: [
      {
        id: '0e48347e-29ea-49fd-aab2-85604f0fadb5',
        name: 'Action',
        description:
          'Fast-paced stories driven by combat, chases and high-stakes physical conflict.',
      },
      {
        id: 'd851ec0a-5ee7-4beb-978b-34c547bb67ec',
        name: 'Adventure',
        description:
          'Journeys and quests through unfamiliar places, full of exploration and risk.',
      },
    ],
  },
];

const mangaFixture = mangasFixture[1]!;

const mangaId = mangaFixture.id;

const mountedWrappers: Array<{ unmount: () => void }> = [];

const mountPage = async () => {
  const wrapper = await mountSuspended(MangaPage, {
    route: `/mangas/${mangaId}`,
  });

  mountedWrappers.push(wrapper);

  return wrapper;
};

describe('MangaPage integration', () => {
  beforeEach(() => {
    vi.clearAllMocks();

    clearNuxtData(`manga-${mangaId}`);
  });

  afterEach(() => {
    mountedWrappers.splice(0).forEach((wrapper) => wrapper.unmount());
  });

  it('requests the manga endpoint with the route id', async () => {
    apiMock.mockResolvedValue(mangaFixture);

    await mountPage();

    await vi.waitFor(() =>
      expect(apiMock).toHaveBeenCalledWith(`/mangas/${mangaId}`),
    );
  });

  it('renders the manga returned by the API', async () => {
    apiMock.mockResolvedValue(mangaFixture);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(true),
    );

    expect(wrapper.find('h2').text()).toBe('Vagabond');

    expect(wrapper.find('h3').exists()).toBe(false);

    expect(wrapper.text()).toContain(mangaFixture.synopsis);

    expect(wrapper.text()).toContain('9.27');

    expect(wrapper.text()).toContain('327 chapters');

    expect(wrapper.text()).toContain('37 volumes');

    expect(wrapper.text()).toContain('Finished');

    expect(wrapper.text()).toContain('Sep 03, 1988 - May 21, 2015');

    expect(wrapper.text()).toContain('Action');

    expect(wrapper.text()).toContain('Adventure');

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });

  it('renders the other manga returned by the API', async () => {
    apiMock.mockResolvedValue(mangasFixture[0]);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(true),
    );

    expect(wrapper.find('h2').text()).toBe(mangasFixture[0]!.title);

    expect(wrapper.text()).toContain('8.88');

    expect(wrapper.text()).toContain('281 chapters');

    expect(wrapper.text()).toContain('28 volumes');

    expect(wrapper.text()).toContain('May 19, 2015 - Nov 02, 2022');

    expect(wrapper.text()).toContain('Comedy');

    expect(wrapper.text()).toContain('Romance');
  });

  it('shows the progress bar and no manga while the request is in flight', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const wrapper = await mountPage();

    await vi.waitFor(
      () => expect(wrapper.findComponent(UProgress).exists()).toBe(true),
      { timeout: 2000 },
    );

    expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);
  });

  it('shows the error state and no manga when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('Network error'));

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findComponent(UEmpty).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('Unable to load manga');

    expect(wrapper.text()).toContain(
      'Something went wrong while fetching the manga details.',
    );

    expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

    expect(wrapper.find('[data-testid="manga-genre"]').exists()).toBe(false);

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);
  });

  it('loads the manga after retrying from the error state', async () => {
    apiMock
      .mockRejectedValueOnce(new Error('Network error'))
      .mockResolvedValueOnce(mangaFixture);

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
      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(true),
    );

    await vi.waitFor(() =>
      expect(wrapper.find('[data-testid="manga-genre"]').exists()).toBe(true),
    );

    expect(wrapper.find('h2').text()).toBe('Vagabond');

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });
});
