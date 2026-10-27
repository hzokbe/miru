import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import {
  AppPageCard,
  CharacterSection,
  CharacterSectionSkeleton,
} from '#components';
import { characterFixtures } from '~~/test/fixtures/characters.ts';
import { fullName } from '~/utils/character-utils.ts';

const { apiMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useRoute', () => () => ({ params: { id: '1' } }));

describe('CharacterSection integration', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('shows the skeleton while loading and the card after the response', async () => {
    let resolve!: (value: (typeof characterFixtures)[number]) => void;

    apiMock.mockReturnValue(
      new Promise<(typeof characterFixtures)[number]>((r) => {
        resolve = r;
      }),
    );

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.findComponent(CharacterSectionSkeleton).exists()).toBe(true);

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    resolve(characterFixtures[0]!);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.findComponent(CharacterSectionSkeleton).exists()).toBe(
      false,
    );

    expect(apiMock).toHaveBeenCalledWith('/characters/1');
  });

  it('renders name, description, image and badges from the API data', async () => {
    const character = characterFixtures[0]!;

    apiMock.mockResolvedValue(character);

    const wrapper = await mountSuspended(CharacterSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    const card = wrapper.findComponent(AppPageCard);

    const title = fullName(character.name, character.lastName);

    expect(card.props('title')).toBe(title);

    expect(card.props('description')).toBe(
      character.description ?? 'No description',
    );

    expect(card.props('image')).toEqual({
      alt: title,
      src: character.imageSource,
    });

    expect(wrapper.find('[data-testid="character-role"]').text()).toBe(
      character.role,
    );

    expect(wrapper.find('[data-testid="character-gender"]').text()).toBe(
      character.gender,
    );

    expect(wrapper.find('[data-testid="character-age"]').text()).toBe(
      String(character.age ?? '?'),
    );
  });

  it('renders the fallback age when the character has no age', async () => {
    apiMock.mockResolvedValue({ ...characterFixtures[0]!, age: null });

    const wrapper = await mountSuspended(CharacterSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.find('[data-testid="character-age"]').text()).toBe('?');
  });

  it('renders the fallback description when there is no description', async () => {
    apiMock.mockResolvedValue({
      ...characterFixtures[0]!,
      description: null,
    });

    const wrapper = await mountSuspended(CharacterSection);

    await vi.waitFor(() =>
      expect(wrapper.findComponent(AppPageCard).exists()).toBe(true),
    );

    expect(wrapper.text()).toContain('No description');
  });

  it('shows the not found alert when the API returns no character', async () => {
    apiMock.mockResolvedValue(null);

    const wrapper = await mountSuspended(CharacterSection);

    await vi.waitFor(() =>
      expect(wrapper.text()).toContain('Character not found'),
    );

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    expect(wrapper.findComponent(CharacterSectionSkeleton).exists()).toBe(
      false,
    );
  });

  it('shows the error alert when the API fails', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const wrapper = await mountSuspended(CharacterSection);

    await vi.waitFor(() =>
      expect(wrapper.text()).toContain(
        'Something went wrong while loading the character details. Please try again.',
      ),
    );

    expect(wrapper.text()).not.toContain('Character not found');

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);

    expect(wrapper.findComponent(CharacterSectionSkeleton).exists()).toBe(
      false,
    );
  });
});
