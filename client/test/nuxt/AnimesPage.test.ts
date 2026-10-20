import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ref, type Ref } from 'vue';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { UEmpty, UProgress } from '#components';
import AnimeCard from '~/components/AnimeCard.vue';
import AnimesPage from '~/pages/animes/index.vue';

const { useAnimesMock, refreshMock } = vi.hoisted(() => ({
  useAnimesMock: vi.fn(),
  refreshMock: vi.fn(),
}));

mockNuxtImport('useAnimes', () => useAnimesMock);

mockNuxtImport('useDelay', () => (source: Ref<boolean>) => source);

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
    genres: [
      {
        id: '9cb2ae6c-2989-41e9-942b-898d6816cb9e',
        name: 'Drama',
        description:
          'Emotionally driven stories centered on realistic conflicts and character development.',
      },
      {
        id: 'd851ec0a-5ee7-4beb-978b-34c547bb67ec',
        name: 'Adventure',
        description:
          'Journeys and quests through unfamiliar places, full of exploration and risk.',
      },
      {
        id: '45373347-01a1-4bfb-bb07-c43450ce1dbe',
        name: 'Fantasy',
        description:
          'Stories set in worlds with magic, mythical creatures and supernatural elements.',
      },
    ],
  },
] as unknown as AnimeResponse[];

const mockUseAnimes = (state: {
  animes?: AnimeResponse[];
  status: 'idle' | 'pending' | 'success' | 'error';
}) => {
  useAnimesMock.mockReturnValue({
    animes: ref(state.animes ?? []),
    status: ref(state.status),
    refresh: refreshMock,
  });
};

describe('AnimesPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('success', () => {
    it('renders the section title', async () => {
      mockUseAnimes({ animes: animesFixture, status: 'success' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.find('h2').text()).toBe('Top animes');
    });

    it('renders one card per anime', async () => {
      mockUseAnimes({ animes: animesFixture, status: 'success' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(2);

      expect(wrapper.text()).toContain('Steins;Gate');

      expect(wrapper.text()).toContain('Sousou no Frieren');
    });

    it('passes the anime data to each card', async () => {
      mockUseAnimes({ animes: animesFixture, status: 'success' });

      const wrapper = await mountSuspended(AnimesPage);

      const [firstCard] = wrapper.findAllComponents(AnimeCard);

      expect(firstCard?.props()).toMatchObject({
        id: '02e9c6be-06e0-479c-9f39-f02c42d02140',
        title: 'Steins;Gate',
        episodes: 24,
        status: 'FinishedAiring',
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
      });
    });

    it('renders the section without cards when the list is empty', async () => {
      mockUseAnimes({ animes: [], status: 'success' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.find('h2').text()).toBe('Top animes');

      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);
    });

    it('does not render the progress bar or the error state', async () => {
      mockUseAnimes({ animes: animesFixture, status: 'success' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('loading', () => {
    it.each(['idle', 'pending'] as const)(
      'renders the progress bar when the status is %s',
      async (status) => {
        mockUseAnimes({ status });

        const wrapper = await mountSuspended(AnimesPage);

        expect(wrapper.findComponent(UProgress).exists()).toBe(true);
      },
    );

    it('keeps the section title while loading', async () => {
      mockUseAnimes({ status: 'pending' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.find('h2').text()).toBe('Top animes');
    });

    it('does not render cards or the error state while loading', async () => {
      mockUseAnimes({ status: 'pending' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('error', () => {
    it('renders the error state', async () => {
      mockUseAnimes({ status: 'error' });

      const wrapper = await mountSuspended(AnimesPage);

      const empty = wrapper.findComponent(UEmpty);

      expect(empty.exists()).toBe(true);

      expect(empty.props()).toMatchObject({
        title: 'Unable to load animes',
        description: 'Something went wrong while fetching the list.',
        icon: 'i-lucide-circle-x',
      });
    });

    it('keeps the section title on error', async () => {
      mockUseAnimes({ status: 'error' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.find('h2').text()).toBe('Top animes');
    });

    it('does not render cards or the progress bar on error', async () => {
      mockUseAnimes({ status: 'error' });

      const wrapper = await mountSuspended(AnimesPage);

      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);
    });

    it('refreshes the list when the retry button is clicked', async () => {
      mockUseAnimes({ status: 'error' });

      const wrapper = await mountSuspended(AnimesPage);

      const retryButton = wrapper
        .findAll('button')
        .find((button) => button.text() === 'Try again');

      expect(retryButton).toBeDefined();

      await retryButton?.trigger('click');

      expect(refreshMock).toHaveBeenCalledTimes(1);
    });
  });

  it('sets the page title', async () => {
    mockUseAnimes({ animes: animesFixture, status: 'success' });

    await mountSuspended(AnimesPage);

    await vi.waitFor(() => expect(document.title).toBe('Miru - Animes'));
  });
});
