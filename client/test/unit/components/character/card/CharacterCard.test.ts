import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppCard, CharacterCard } from '#components';
import { createCharacterFixture } from '~~/test/fixtures/characters.ts';

const character = createCharacterFixture();

const props = {
  id: character.id,
  name: character.name,
  lastName: character.lastName,
  age: character.age,
  gender: character.gender,
  role: character.role,
  description: character.description,
  imageSource: character.imageSource,
};

describe('CharacterCard', () => {
  it('renders the full name as title', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props });

    expect(wrapper.find('h3').text()).toBe('Miyuki Shirogane');
  });

  it('renders only the name when there is no last name', async () => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...props, lastName: null },
    });

    expect(wrapper.find('h3').text()).toBe('Miyuki');
  });

  it('renders role, gender and age badges', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props });

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
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...props, age: null },
    });

    expect(wrapper.find('[data-testid="character-age"]').text()).toBe('?');
  });

  it.each([
    ['Female', 'i-lucide-venus'],
    ['Male', 'i-lucide-mars'],
    ['Unknown', 'i-lucide-venus-mars'],
  ])('uses the right icon for %s', async (gender, icon) => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...props, gender: gender as typeof props.gender },
    });

    const badges = wrapper.findComponent(AppCard).props('badges');

    expect(badges.find((badge) => badge.id === 'character-gender')?.icon).toBe(
      icon,
    );
  });

  it('passes the image with the full name as alt', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props });

    expect(wrapper.findComponent(AppCard).props('image')).toEqual({
      alt: 'Miyuki Shirogane',
      src: null,
    });
  });

  it('links to the character page', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props });

    expect(wrapper.find('h3 a').attributes('href')).toBe(
      '/characters/' + character.id,
    );
  });
});
