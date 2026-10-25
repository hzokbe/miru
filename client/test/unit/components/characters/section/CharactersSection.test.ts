import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { CharacterCard, CharactersSection } from '#components';
import { characterFixtures } from '~~/test/fixtures/characters.ts';

describe('CharactersSection', () => {
  it('renders the section title', async () => {
    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: characterFixtures },
    });

    expect(wrapper.text()).toContain('Characters');
  });

  it('renders one card per character', async () => {
    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: characterFixtures },
    });

    const cards = wrapper.findAllComponents(CharacterCard);

    expect(cards).toHaveLength(characterFixtures.length);

    expect(cards.map((card) => card.props('id'))).toEqual(
      characterFixtures.map((character) => character.id),
    );
  });

  it('passes the character data to each card', async () => {
    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: characterFixtures },
    });

    const [first] = characterFixtures;

    const card = wrapper.findAllComponents(CharacterCard)[0]!;

    expect(card.props('name')).toBe(first!.name);

    expect(card.props('lastName')).toBe(first!.lastName);

    expect(card.props('age')).toBe(first!.age);

    expect(card.props('gender')).toBe(first!.gender);

    expect(card.props('role')).toBe(first!.role);

    expect(card.props('description')).toBe(first!.description);

    expect(card.props('imageSource')).toBe(first!.imageSource);
  });

  it('renders no cards when the list is empty', async () => {
    const wrapper = await mountSuspended(CharactersSection, {
      props: { characters: [] },
    });

    expect(wrapper.findAllComponents(CharacterCard)).toHaveLength(0);
  });
});
