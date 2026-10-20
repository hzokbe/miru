import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ref, type Ref } from 'vue';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { UEmpty, UProgress } from '#components';
import AppImage from '~/components/AppImage.vue';
import AnimePage from '~/pages/animes/[id].vue';

const { useAnimeMock, refreshMock } = vi.hoisted(() => ({
  useAnimeMock: vi.fn(),
  refreshMock: vi.fn(),
}));

mockNuxtImport('useAnime', () => useAnimeMock);

mockNuxtImport('useDelay', () => (source: Ref<boolean>) => source);

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
} as unknown as AnimeResponse;

const mockUseAnime = (state: {
  anime?: AnimeResponse | null;
  status: 'idle' | 'pending' | 'success' | 'error';
}) => {
  useAnimeMock.mockReturnValue({
    anime: ref(state.anime ?? null),
    status: ref(state.status),
    refresh: refreshMock,
  });
};

const mountPage = () =>
  mountSuspended(AnimePage, { route: `/animes/${animeId}` });

describe('AnimePage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('requests the anime from the route id', async () => {
    mockUseAnime({ anime: animeFixture, status: 'success' });

    await mountPage();

    expect(useAnimeMock).toHaveBeenCalledWith(animeId);
  });

  describe('success', () => {
    it('renders the title and the japanese title', async () => {
      mockUseAnime({ anime: animeFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.find('h2').text()).toBe('Steins;Gate');

      expect(wrapper.find('h3').text()).toBe('シュタインズ・ゲート');
    });

    it('does not render the japanese title when it is null', async () => {
      mockUseAnime({
        anime: { ...animeFixture, japaneseTitle: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.find('h3').exists()).toBe(false);
    });

    it('passes the title and image source to AppImage with the md size', async () => {
      mockUseAnime({ anime: animeFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.findComponent(AppImage).props()).toMatchObject({
        alt: 'Steins;Gate',
        src: '02e9c6be-06e0-479c-9f39-f02c42d02140.jpg',
        size: 'md',
      });
    });

    it('renders the synopsis', async () => {
      mockUseAnime({ anime: animeFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.text()).toContain(animeFixture.synopsis);

      expect(wrapper.text()).not.toContain('No synopsis');
    });

    it('renders a fallback when the synopsis is null', async () => {
      mockUseAnime({
        anime: { ...animeFixture, synopsis: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.text()).toContain('No synopsis');
    });

    it('renders no genres when anime has no genres', async () => {
      mockUseAnime({
        anime: { ...animeFixture, genres: [] },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.findAll('[data-testid="anime-genre"]')).toHaveLength(0);
    });

    it('renders the score, type, episodes, status, aired dates and genres', async () => {
      mockUseAnime({ anime: animeFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="anime-score"]').text()).toBe('9.07');

      expect(wrapper.get('[data-testid="anime-type"]').text()).toBe('TV');

      expect(wrapper.get('[data-testid="anime-episodes"]').text()).toBe(
        '24 episodes',
      );

      expect(wrapper.get('[data-testid="anime-status"]').text()).toBe(
        'Finished Airing',
      );

      expect(wrapper.get('[data-testid="anime-aired"]').text()).toBe(
        'Apr 06, 2011 - Sep 14, 2011',
      );

      expect(wrapper.findAll('[data-testid="anime-genre"]')).toHaveLength(3);
    });

    it.each([
      ['Tv', 'TV'],
      ['Ona', 'ONA'],
      ['TvSpecial', 'TV Special'],
    ] as const)('formats the type %s as %s', async (type, expected) => {
      mockUseAnime({
        anime: { ...animeFixture, type },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="anime-type"]').text()).toBe(expected);
    });

    it('renders fallbacks when score and episodes are null', async () => {
      mockUseAnime({
        anime: { ...animeFixture, score: null, episodes: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="anime-score"]').text()).toBe('0');

      expect(wrapper.get('[data-testid="anime-episodes"]').text()).toBe(
        '0 episodes',
      );
    });

    it('renders a question mark when the aired dates are null', async () => {
      mockUseAnime({
        anime: { ...animeFixture, airedFrom: null, airedTo: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="anime-aired"]').text()).toBe('? - ?');
    });

    it('renders a question mark when the aired dates are invalid', async () => {
      mockUseAnime({
        anime: { ...animeFixture, airedFrom: 'invalid', airedTo: 'invalid' },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="anime-aired"]').text()).toBe('? - ?');
    });

    it('renders an open ended range when the anime is still airing', async () => {
      mockUseAnime({
        anime: { ...animeFixture, airedTo: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="anime-aired"]').text()).toBe(
        'Apr 06, 2011 - ?',
      );
    });

    it('does not render the progress bar or the error state', async () => {
      mockUseAnime({ anime: animeFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });

    it('renders nothing when the request succeeds without an anime', async () => {
      mockUseAnime({ anime: null, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(false);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('loading', () => {
    it.each(['idle', 'pending'] as const)(
      'renders the progress bar when the status is %s',
      async (status) => {
        mockUseAnime({ status });

        const wrapper = await mountPage();

        expect(wrapper.findComponent(UProgress).exists()).toBe(true);
      },
    );

    it('does not render the anime or the error state while loading', async () => {
      mockUseAnime({ status: 'pending' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('error', () => {
    it('renders the error state', async () => {
      mockUseAnime({ status: 'error' });

      const wrapper = await mountPage();

      const empty = wrapper.findComponent(UEmpty);

      expect(empty.exists()).toBe(true);

      expect(empty.props()).toMatchObject({
        title: 'Unable to load anime',
        description: 'Something went wrong while fetching the anime details.',
        icon: 'i-lucide-circle-x',
      });
    });

    it('does not render the anime or the progress bar on error', async () => {
      mockUseAnime({ status: 'error' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="anime-score"]').exists()).toBe(false);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);
    });

    it('refreshes the anime when the retry button is clicked', async () => {
      mockUseAnime({ status: 'error' });

      const wrapper = await mountPage();

      const retryButton = wrapper
        .findAll('button')
        .find((button) => button.text() === 'Try again');

      expect(retryButton).toBeDefined();

      await retryButton?.trigger('click');

      expect(refreshMock).toHaveBeenCalledTimes(1);
    });
  });

  describe('seo', () => {
    it('sets the page title to the anime title', async () => {
      mockUseAnime({ anime: animeFixture, status: 'success' });

      await mountPage();

      await vi.waitFor(() => expect(document.title).toBe('Steins;Gate'));
    });

    it('falls back to the default title when there is no anime', async () => {
      mockUseAnime({ status: 'pending' });

      await mountPage();

      await vi.waitFor(() => expect(document.title).toBe('Anime Details'));
    });
  });
});
