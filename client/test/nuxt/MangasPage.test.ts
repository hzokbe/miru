import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ref, type Ref } from 'vue';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { UEmpty, UProgress } from '#components';
import MangaCard from '~/components/MangaCard.vue';
import MangasPage from '~/pages/mangas/index.vue';

const { useMangasMock, refreshMock } = vi.hoisted(() => ({
  useMangasMock: vi.fn(),
  refreshMock: vi.fn(),
}));

mockNuxtImport('useMangas', () => useMangasMock);

mockNuxtImport('useDelay', () => (source: Ref<boolean>) => source);

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
] as unknown as MangaResponse[];

const mockUseMangas = (state: {
  mangas?: MangaResponse[];
  status: 'idle' | 'pending' | 'success' | 'error';
}) => {
  useMangasMock.mockReturnValue({
    mangas: ref(state.mangas ?? []),
    status: ref(state.status),
    refresh: refreshMock,
  });
};

describe('MangasPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('success', () => {
    it('renders the section title', async () => {
      mockUseMangas({ mangas: mangasFixture, status: 'success' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.find('h2').text()).toBe('Top mangas');
    });

    it('renders one card per manga', async () => {
      mockUseMangas({ mangas: mangasFixture, status: 'success' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(2);

      expect(wrapper.text()).toContain(
        'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
      );

      expect(wrapper.text()).toContain('Vagabond');
    });

    it('passes the manga data to each card', async () => {
      mockUseMangas({ mangas: mangasFixture, status: 'success' });

      const wrapper = await mountSuspended(MangasPage);

      const [firstCard] = wrapper.findAllComponents(MangaCard);

      expect(firstCard?.props()).toMatchObject({
        id: '1bad1470-01d4-4664-80bd-90cced7a6279',
        title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
        chapters: 281,
        volumes: 28,
        status: 'Finished',
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
      });
    });

    it('renders the section without cards when the list is empty', async () => {
      mockUseMangas({ mangas: [], status: 'success' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.find('h2').text()).toBe('Top mangas');

      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);
    });

    it('does not render the progress bar or the error state', async () => {
      mockUseMangas({ mangas: mangasFixture, status: 'success' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('loading', () => {
    it.each(['idle', 'pending'] as const)(
      'renders the progress bar when the status is %s',
      async (status) => {
        mockUseMangas({ status });

        const wrapper = await mountSuspended(MangasPage);

        expect(wrapper.findComponent(UProgress).exists()).toBe(true);
      },
    );

    it('keeps the section title while loading', async () => {
      mockUseMangas({ status: 'pending' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.find('h2').text()).toBe('Top mangas');
    });

    it('does not render cards or the error state while loading', async () => {
      mockUseMangas({ status: 'pending' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('error', () => {
    it('renders the error state', async () => {
      mockUseMangas({ status: 'error' });

      const wrapper = await mountSuspended(MangasPage);

      const empty = wrapper.findComponent(UEmpty);

      expect(empty.exists()).toBe(true);

      expect(empty.props()).toMatchObject({
        title: 'Unable to load mangas',
        description: 'Something went wrong while fetching the list.',
        icon: 'i-lucide-circle-x',
      });
    });

    it('keeps the section title on error', async () => {
      mockUseMangas({ status: 'error' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.find('h2').text()).toBe('Top mangas');
    });

    it('does not render cards or the progress bar on error', async () => {
      mockUseMangas({ status: 'error' });

      const wrapper = await mountSuspended(MangasPage);

      expect(wrapper.findAllComponents(MangaCard)).toHaveLength(0);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);
    });

    it('refreshes the list when the retry button is clicked', async () => {
      mockUseMangas({ status: 'error' });

      const wrapper = await mountSuspended(MangasPage);

      const retryButton = wrapper
        .findAll('button')
        .find((button) => button.text() === 'Try again');

      expect(retryButton).toBeDefined();

      await retryButton?.trigger('click');

      expect(refreshMock).toHaveBeenCalledTimes(1);
    });
  });

  it('sets the page title', async () => {
    mockUseMangas({ mangas: mangasFixture, status: 'success' });

    await mountSuspended(MangasPage);

    await vi.waitFor(() => expect(document.title).toBe('Miru - Mangas'));
  });
});
