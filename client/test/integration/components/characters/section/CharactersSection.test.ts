import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { CharacterCard, CharactersSection } from '#components';
import { characterFixtures } from '~~/test/fixtures/characters.ts';

const { navigateToMock } = vi.hoisted(() => ({ navigateToMock: vi.fn() }));

mockNuxtImport('navigateTo', () => navigateToMock);

describe('CharactersSection integration', () => {
  beforeEach(() => {
    navigateToMock.mockReset();
  });

  it('renders names and badges for every character', async () => {
    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: characterFixtures },
    });

    const cards = wrapper.findAllComponents(CharacterCard);

    expect(cards).toHaveLength(characterFixtures.length);

    characterFixtures.forEach((character, index) => {
      const fullName = [character.name, character.lastName]
        .filter(Boolean)
        .join(' ');

      const card = cards[index]!;

      expect(card.find('h3').text()).toBe(fullName);

      expect(card.find('[data-testid="character-role"]').text()).toBe(
        character.role,
      );

      expect(card.find('[data-testid="character-gender"]').text()).toBe(
        character.gender,
      );

      expect(card.find('[data-testid="character-age"]').text()).toBe(
        character.age ?? '?',
      );
    });
  });

  it('renders a card without last name using only the name', async () => {
    const frieren = characterFixtures.find(
      (character) => character.name === 'Frieren',
    )!;

    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: [frieren] },
    });

    expect(wrapper.find('h3').text()).toBe('Frieren');

    expect(wrapper.find('[data-testid="character-age"]').text()).toBe('1000+');
  });

  it('navigates to the clicked character page', async () => {
    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: characterFixtures },
    });

    const target = characterFixtures[2]!;

    await wrapper
      .findAllComponents(CharacterCard)[2]!
      .find('h3')
      .trigger('click');

    expect(navigateToMock).toHaveBeenCalledWith('/characters/' + target.id);
  });
});
