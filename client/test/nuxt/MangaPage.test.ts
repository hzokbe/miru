import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ref, type Ref } from 'vue';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { UEmpty, UProgress } from '#components';
import AppImage from '~/components/AppImage.vue';
import MangaPage from '~/pages/mangas/[id].vue';

const { useMangaMock, refreshMock } = vi.hoisted(() => ({
  useMangaMock: vi.fn(),
  refreshMock: vi.fn(),
}));

mockNuxtImport('useManga', () => useMangaMock);

mockNuxtImport('useDelay', () => (source: Ref<boolean>) => source);

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const mangaId = 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a';

const mangaFixture = {
  id: mangaId,
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

      expect(wrapper.find('h2').text()).toBe('Vagabond');
    });

    it('does not render the japanese title when it is null', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.find('h3').exists()).toBe(false);
    });

    it('renders the japanese title when present', async () => {
      mockUseManga({
        manga: { ...mangaFixture, japaneseTitle: 'バガボンド' },
        status: 'success',
      });

      const wrapper = await mountPage();

      expect(wrapper.find('h3').text()).toBe('バガボンド');
    });

    it('passes the title and image source to AppImage with the md size', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.findComponent(AppImage).props()).toMatchObject({
        alt: 'Vagabond',
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

    it('renders the score, chapters, volumes, status and published dates', async () => {
      mockUseManga({ manga: mangaFixture, status: 'success' });

      const wrapper = await mountPage();

      expect(wrapper.get('[data-testid="manga-score"]').text()).toBe('9.27');

      expect(wrapper.get('[data-testid="manga-chapters"]').text()).toBe(
        '327 chapters',
      );

      expect(wrapper.get('[data-testid="manga-volumes"]').text()).toBe(
        '37 volumes',
      );

      expect(wrapper.get('[data-testid="manga-status"]').text()).toBe(
        'Finished',
      );

      expect(wrapper.get('[data-testid="manga-published"]').text()).toBe(
        'Sep 03, 1988 - May 21, 2015',
      );
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
        'Sep 03, 1988 - ?',
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

      expect(wrapper.findComponent(UProgress).exists()).toBe(false);

      expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
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

    it('does not render the manga or the error state while loading', async () => {
      mockUseManga({ status: 'pending' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

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

    it('does not render the manga or the progress bar on error', async () => {
      mockUseManga({ status: 'error' });

      const wrapper = await mountPage();

      expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

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

      await vi.waitFor(() => expect(document.title).toBe('Vagabond'));
    });

    it('falls back to the default title when there is no manga', async () => {
      mockUseManga({ status: 'pending' });

      await mountPage();

      await vi.waitFor(() => expect(document.title).toBe('Manga Details'));
    });
  });
});
