import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { clearNuxtData } from '#app';
import { UEmpty, UProgress } from '#components';
import CharacterCard from '~/components/CharacterCard.vue';
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
    characters: [
      {
        id: '33333333-3333-4333-8333-333333333333',
        name: 'Kaguya',
        lastName: 'Shinomiya',
        japaneseName: 'かぐや',
        japaneseLastName: '四宮',
        age: null,
        gender: 'Female',
        role: 'Main',
        description:
          'The student council vice president and heiress of the Shinomiya group, gifted but sheltered and proud.',
        imageSource: '33333333-3333-4333-8333-333333333333.jpg',
      },
      {
        id: '44444444-4444-4444-8444-444444444444',
        name: 'Miyuki',
        lastName: 'Shirogane',
        japaneseName: '御行',
        japaneseLastName: '白銀',
        age: '17',
        gender: 'Male',
        role: 'Main',
        description:
          'The hardworking student council president, who refuses to confess his love for Kaguya first.',
        imageSource: '44444444-4444-4444-8444-444444444444.jpg',
      },
    ],
  },
  {
    id: 'cfd9a9ef-e1e2-40f6-ae13-d7f0feacc22a',
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
    characters: [
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

    expect(wrapper.find('h2').text()).toBe('Sousou no Frieren');

    expect(wrapper.find('h2 + h3').exists()).toBe(false);

    expect(wrapper.text()).toContain(mangaFixture.synopsis);

    expect(wrapper.text()).toContain('8.87');

    expect(wrapper.text()).toContain('147 chapters');

    expect(wrapper.text()).toContain('15 volumes');

    expect(wrapper.text()).toContain('On Hiatus');

    expect(wrapper.text()).toContain('Apr 28, 2020 - Oct 15, 2025');

    expect(wrapper.text()).toContain('Adventure');

    expect(wrapper.text()).toContain('Fantasy');

    expect(wrapper.findComponent(UProgress).exists()).toBe(false);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });

  it('renders the characters returned by the API', async () => {
    apiMock.mockResolvedValue(mangaFixture);

    const wrapper = await mountPage();

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(2),
    );

    expect(wrapper.text()).toContain('Characters');

    expect(wrapper.text()).toContain('Frieren');

    expect(wrapper.text()).toContain('Fern');
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

    expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(2);

    expect(wrapper.text()).toContain('Kaguya');

    expect(wrapper.text()).toContain('Miyuki');
  });

  it('shows the progress bar and no manga while the request is in flight', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const wrapper = await mountPage();

    await vi.waitFor(
      () => expect(wrapper.findComponent(UProgress).exists()).toBe(true),
      { timeout: 2000 },
    );

    expect(wrapper.find('[data-testid="manga-score"]').exists()).toBe(false);

    expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);
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

    expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);

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

    expect(wrapper.find('h2').text()).toBe('Sousou no Frieren');

    expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(2);

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(wrapper.findComponent(UEmpty).exists()).toBe(false);
  });
});
