import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import {
  AppPageCard,
  CharacterSection,
  CharacterSectionSkeleton,
} from '#components';
import { characterFixtures } from '~~/test/fixtures/characters.ts';
import { fullName } from '~/utils/character-utils.ts';

const state = vi.hoisted(() => ({
  character: null as unknown,
  loading: false,
  error: null as Error | null,
}));

mockNuxtImport('useRoute', () => () => ({ params: { id: '1' } }));

mockNuxtImport('useCharacter', async () => {
  const { ref } = await import('vue');

  return () => ({
    character: ref(state.character),
    loading: ref(state.loading),
    error: ref(state.error),
  });
});

describe('CharacterSection', () => {
  beforeEach(() => {
    state.character = characterFixtures[1];

    state.loading = false;

    state.error = null;
  });

  it('renders the skeleton while loading', async () => {
    state.loading = true;

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.findComponent(CharacterSectionSkeleton).exists()).toBe(true);

    expect(wrapper.text()).not.toContain('Character not found');
  });

  it('renders the page card with character data', async () => {
    const wrapper = await mountSuspended(CharacterSection);

    const character = characterFixtures[1]!;

    const card = wrapper.findComponent(AppPageCard);

    expect(wrapper.findComponent(CharacterSectionSkeleton).exists()).toBe(
      false,
    );

    expect(card.props('title')).toBe(
      fullName(character.name, character.lastName),
    );

    expect(card.props('description')).toBe(character.description);

    expect(card.props('image')).toEqual({
      alt: fullName(character.name, character.lastName),
      src: character.imageSource,
    });
  });

  it('renders role, gender and age badges', async () => {
    const wrapper = await mountSuspended(CharacterSection);

    const character = characterFixtures[1]!;

    expect(wrapper.find('[data-testid="character-role"]').text()).toBe(
      character.role,
    );

    expect(wrapper.find('[data-testid="character-gender"]').text()).toBe(
      character.gender,
    );

    expect(wrapper.find('[data-testid="character-age"]').text()).toBe(
      character.age,
    );
  });

  it('renders a placeholder when the age is unknown', async () => {
    state.character = { ...characterFixtures[1]!, age: null };

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.find('[data-testid="character-age"]').text()).toBe('?');
  });

  it('renders only the name when there is no last name', async () => {
    state.character = { ...characterFixtures[1]!, lastName: null };

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.findComponent(AppPageCard).props('title')).toBe('Miyuki');
  });

  it('renders the fallback description when there is no description', async () => {
    state.character = { ...characterFixtures[1]!, description: null };

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.findComponent(AppPageCard).props('description')).toBe(
      'No description',
    );
  });

  it('renders the error alert instead of the card on error', async () => {
    state.error = new Error('request failed');

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the character details. Please try again.',
    );

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });

  it('prioritizes the error alert over the not found alert', async () => {
    state.error = new Error('request failed');

    state.character = null;

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.text()).toContain(
      'Something went wrong while loading the character details. Please try again.',
    );

    expect(wrapper.text()).not.toContain('Character not found');
  });

  it('renders the not found alert when there is no character', async () => {
    state.character = null;

    const wrapper = await mountSuspended(CharacterSection);

    expect(wrapper.text()).toContain('Character not found');

    expect(wrapper.findComponent(AppPageCard).exists()).toBe(false);
  });
});
