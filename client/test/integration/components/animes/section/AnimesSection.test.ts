import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { AnimeCard, AnimeCardSkeleton, AnimesSection } from '#components';
import { animeFixtures } from '~~/test/fixtures/animes.ts';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

describe('AnimesSection integration', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData('animes-all');
  });

  it('shows skeletons while loading and cards after the response', async () => {
    let resolve!: (value: typeof animeFixtures) => void;

    apiMock.mockReturnValue(
      new Promise<typeof animeFixtures>((r) => {
        resolve = r;
      }),
    );

    const wrapper = await mountSuspended(AnimesSection);

    expect(wrapper.findAllComponents(AnimeCardSkeleton)).toHaveLength(9);

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);

    resolve(animeFixtures);

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(
        animeFixtures.length,
      ),
    );

    expect(wrapper.findAllComponents(AnimeCardSkeleton)).toHaveLength(0);

    expect(apiMock).toHaveBeenCalledWith('/animes');
  });

  it('renders titles and formatted badges from the API data', async () => {
    apiMock.mockResolvedValue(animeFixtures);

    const wrapper = await mountSuspended(AnimesSection);

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(
        animeFixtures.length,
      ),
    );

    const [first] = animeFixtures;

    const card = wrapper.findAllComponents(AnimeCard)[0]!;

    expect(card.text()).toContain(first!.title);

    expect(card.find('[data-testid="anime-score"]').text()).toBe(
      formatScore(first!.score),
    );

    expect(card.find('[data-testid="anime-type"]').text()).toBe(
      formatType(first!.type),
    );

    expect(card.find('[data-testid="anime-episodes"]').text()).toBe(
      formatEpisodes(first!.episodes),
    );

    expect(card.find('[data-testid="anime-status"]').text()).toBe(
      formatStatus(first!.status),
    );
  });

  it('navigates to the clicked anime page', async () => {
    apiMock.mockResolvedValue(animeFixtures);

    const wrapper = await mountSuspended(AnimesSection);

    await vi.waitFor(() =>
      expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(
        animeFixtures.length,
      ),
    );

    const target = animeFixtures[3]!;

    const card = wrapper.findAllComponents(AnimeCard)[3]!;

    await card.find('h3').trigger('click');

    expect(card.find('a').attributes('href')).toBe(`/animes/${target.id}`);
  });

  it('shows the empty alert when the API returns no animes', async () => {
    apiMock.mockResolvedValue([]);

    const wrapper = await mountSuspended(AnimesSection);

    await vi.waitFor(() => expect(wrapper.text()).toContain('No anime found'));

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);

    expect(wrapper.findAllComponents(AnimeCardSkeleton)).toHaveLength(0);
  });

  it('shows the error alert when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const wrapper = await mountSuspended(AnimesSection);

    await vi.waitFor(() =>
      expect(wrapper.text()).toContain(
        'Something went wrong while loading the anime list.',
      ),
    );

    expect(wrapper.text()).not.toContain('No anime found');

    expect(wrapper.findAllComponents(AnimeCard)).toHaveLength(0);

    expect(wrapper.findAllComponents(AnimeCardSkeleton)).toHaveLength(0);
  });
});
