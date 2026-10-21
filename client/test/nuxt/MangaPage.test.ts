import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ref, type Ref } from 'vue';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { UEmpty, UProgress } from '#components';
import AppImage from '~/components/AppImage.vue';
import CharacterCard from '~/components/CharacterCard.vue';
import MangaPage from '~/pages/mangas/[id].vue';

const { useMangaMock, refreshMock } = vi.hoisted(() => ({
  useMangaMock: vi.fn(),
  refreshMock: vi.fn(),
}));

mockNuxtImport('useManga', () => useMangaMock);

mockNuxtImport('useDelay', () => (source: Ref<boolean>) => source);

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const mangaId = 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a';

const charactersFixture = [
  {
    id: '11111111-1111-4111-8111-111111111111',
    name: 'Frieren',
    lastName: null,
    japaneseName: 'フリーレン',
    japaneseLastName: null,
    age: '1000+',
    gender: 'Female',
    role: 'Main',
    description:
      'A long-lived elf mage who defeated the Demon King and now travels to understand humans and her fallen companions.',
    imageSource: '11111111-1111-4111-8111-111111111111.jpg',
  },
  {
    id: '22222222-2222-4222-8222-222222222222',
    name: 'Fern',
    lastName: null,
    japaneseName: 'フェルン',
    japaneseLastName: null,
    age: null,
    gender: 'Female',
    role: 'Main',
    description:
      "Heiter's adopted daughter and Frieren's apprentice, serious and mature, who looks after the party like a mother.",
    imageSource: '22222222-2222-4222-8222-222222222222.jpg',
  },
];

const mangaFixture = {
  id: mangaId,
  title: 'Sousou no Frieren',
  japaneseTitle: null,
  synopsis:
    "After a hero's party defeats the Demon King, the long-lived elf mage Frieren outlives her old companions. Realizing she never truly got to know them, she sets out on a new journey with a young apprentice to revisit the places they once shared.",
  chapters: 147,
  volumes: 15,
  status: 'OnHiatus',
  publishedFrom: '2020-04-28',
  publishedTo: '2025-10-15',
  score: 8.87,
  imageSource: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a.jpg',
  genres: [
    {
      id: '0e48347e-29ea-49fd-aab2-85604f0fadb5',
      name: 'Adventure',
      description:
        'Journeys and quests through unfamiliar places, full of exploration and risk.',
    },
    {
      id: 'd851ec0a-5ee7-4beb-978b-34c547bb67ec',
      name: 'Fantasy',
      description:
        'Stories set in imaginary worlds with magic, mythical creatures and supernatural elements.',
    },
  ],
  characters: charactersFixture,
} as unknown as MangaResponse;

const mockUseManga = (state: {
  manga?: MangaResponse | null;
  status: 'idle' | 'pending' | 'success' | 'error';
}) => {
  useMangaMock.mockReturnValue({
    manga: ref(state.manga ?? null),
    status: ref(state.status),
    refresh: refreshMock,
  });
};

const mountPage = () =>
  mountSuspended(MangaPage, { route: `/mangas/${mangaId}` });

describe('MangaPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('requests the manga from the route id', async () => {
    mockUseManga({ manga: mangaFixture, status: 'success' });

    await mountPage();

    expect(useMangaMock).toHaveBeenCalledWith(mangaId);
  });

  describe('success', () => {
    it('renders the title', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.find('h2').text()).toBe('Sousou no Frieren');
    });

    it('does not render the japanese title when it is null', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.find('h2 + h3').exists()).toBe(false);
    });

    it('renders the japanese title when present', async () => {
      mockUseManga({
        manga: { ...mangaFixture, japaneseTitle: '葬送のフリーレン' },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.find('h2 + h3').text()).toBe('葬送のフリーレン');
    });

    it('passes the title and image source to AppImage with the md size', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.findComponent(AppImage).props()).toMatchObject({
        alt: 'Sousou no Frieren',
        src: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a.jpg',
        size: 'md',
      });
    });

    it('renders the synopsis', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.text()).toContain(mangaFixture.synopsis);

      expect(wrapper.text()).not.toContain('No synopsis');
    });

    it('renders a fallback when the synopsis is null', async () => {
      mockUseManga({
        manga: { ...mangaFixture, synopsis: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.text()).toContain('No synopsis');
    });

    it('renders the score, chapters, volumes, status, published dates and genres', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-score"]').text()).toBe('8.87');

      expect(wrapper.get('[data-testid="manga-chapters"]').text()).toBe(
        '147 chapters',
      );

      expect(wrapper.get('[data-testid="manga-volumes"]').text()).toBe(
        '15 volumes',
      );

      expect(wrapper.get('[data-testid="manga-status"]').text()).toBe(
        'On Hiatus',
      );

      expect(wrapper.get('[data-testid="manga-published"]').text()).toBe(
        'Apr 28, 2020 - Oct 15, 2025',
      );

      expect(wrapper.findAll('[data-testid="manga-genre"]')).toHaveLength(2);
    });

    it('does not render genres when there are no genres', async () => {
      mockUseManga({
        manga: { ...mangaFixture, genres: [] },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.findAll('[data-testid="manga-genre"]')).toHaveLength(0);
    });

    it('does not render genres when genres is null', async () => {
      mockUseManga({
        manga: { ...mangaFixture, genres: null } as unknown as MangaResponse,
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.findAll('[data-testid="manga-genre"]')).toHaveLength(0);
    });

    it.each([
      ['Finished', 'Finished'],
      ['OnHiatus', 'On Hiatus'],
      ['CurrentlyPublishing', 'Currently Publishing'],
    ] as const)('formats the status %s as %s', async (status, expected) => {
      mockUseManga({
        manga: { ...mangaFixture, status } as unknown as MangaResponse,
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-status"]').text()).toBe(expected);
    });

    it('renders fallbacks when score, chapters and volumes are null', async () => {
      mockUseManga({
        manga: { ...mangaFixture, score: null, chapters: null, volumes: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-score"]').text()).toBe('0');

      expect(wrapper.get('[data-testid="manga-chapters"]').text()).toBe(
        '0 chapters',
      );

      expect(wrapper.get('[data-testid="manga-volumes"]').text()).toBe(
        '0 volumes',
      );
    });

    it('renders a question mark when the published dates are null', async () => {
      mockUseManga({
        manga: { ...mangaFixture, publishedFrom: null, publishedTo: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-published"]').text()).toBe(
        '? - ?',
      );
    });

    it('renders a question mark when the published dates are invalid', async () => {
      mockUseManga({
        manga: {
          ...mangaFixture,
          publishedFrom: 'invalid',
          publishedTo: 'invalid',
        },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-published"]').text()).toBe(
        '? - ?',
      );
    });

    it('renders an open ended range when the manga is still publishing', async () => {
      mockUseManga({
        manga: { ...mangaFixture, publishedTo: null },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-published"]').text()).toBe(
        'Apr 28, 2020 - ?',
      );
    });

    it('does not render the progress bar or the error state', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });

    it('renders nothing when the request succeeds without a manga', async () => {
      mockUseManga({ manga: null, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

      expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('characters', () => {
    it('renders the characters section title', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.text()).toContain('Characters');
    });

    it('renders one CharacterCard per character', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(2);
    });

    it('passes the character data to each CharacterCard', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      const cards = wrapper.findAllComponents(CharacterCard);

      expect(cards[0]!.props()).toMatchObject({
        id: '11111111-1111-4111-8111-111111111111',
        name: 'Frieren',
        lastName: null,
        age: '1000+',
        gender: 'Female',
        role: 'Main',
        description:
          'A long-lived elf mage who defeated the Demon King and now travels to understand humans and her fallen companions.',
        imageSource: '11111111-1111-4111-8111-111111111111.jpg',
      });

      expect(cards[1]!.props()).toMatchObject({
        id: '22222222-2222-4222-8222-222222222222',
        name: 'Fern',
        lastName: null,
        age: null,
        gender: 'Female',
        role: 'Main',
        description:
          "Heiter's adopted daughter and Frieren's apprentice, serious and mature, who looks after the party like a mother.",
        imageSource: '22222222-2222-4222-8222-222222222222.jpg',
      });
    });

    it('does not render any CharacterCard when there are no characters', async () => {
      mockUseManga({
        manga: { ...mangaFixture, characters: [] },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);
    });
  });

  describe('loading', () => {
    it.each(['idle', 'pending'] as const)(
      'renders the progress bar when the status is %s',
      async (status) => {
        mockUseManga({ status });

        const wrapper = await mountPage();

        expect(wrapper.findComponent(UProgress).exists()).toBe(true);
      },
    );

    it('does not render the manga, the characters or the error state while loading', async () => {
      mockUseManga({ status: 'pending' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

      expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
    });
  });

  describe('error', () => {
    it('renders the error state', async () => {
      mockUseManga({ status: 'error' });

      const wrapper = await mountPage();

      const empty = wrapper.findComponent(UEmpty);

      expect(empty.exists()).toBe(true);

      expect(empty.props()).toMatchObject({
        title: 'Unable to load manga',
        description: 'Something went wrong while fetching the manga details.',
        icon: 'i-lucide-circle-x',
      });
    });

    it('does not render the manga, the characters or the progress bar on error', async () => {
      mockUseManga({ status: 'error' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

      expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);
    });

    it('refreshes the manga when the retry button is clicked', async () => {
      mockUseManga({ status: 'error' });

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
    it('sets the page title to the manga title', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      await mountPage();

      await vi.waitFor(() => expect(document.title).toBe('Sousou no Frieren'));
    });

    it('falls back to the default title when there is no manga', async () => {
      mockUseManga({ status: 'pending' });

      await mountPage();

      await vi.waitFor(() => expect(document.title).toBe('Manga Details'));
    });
  });
});
