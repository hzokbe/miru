import { describe, expect, it, vi } from 'vitest';
import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import CharacterCard from '~/components/CharacterCard.vue';
import AppImage from '~/components/AppImage.vue';

mockNuxtImport('useAPIHealth', () => () => ({ checkHealth: vi.fn() }));

const baseProps = {
  id: '7d2c4a9e-3b1f-4e6a-8c05-9a1e5f0b2d34',
  name: 'Frieren',
  lastName: '',
  age: '1000+',
  gender: 'female',
  role: 'Main',
  description:
    'A long-lived elf mage who defeated the Demon King and now travels to understand humans and her fallen companions.',
  imageSource: '7d2c4a9e-3b1f-4e6a-8c05-9a1e5f0b2d34.jpg',
};

describe('CharacterCard', () => {
  it('renders the name when there is no last name', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props: baseProps });

    expect(wrapper.find('h3').text()).toBe('Frieren');
  });

  it('renders the full name when the last name is provided', async () => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...baseProps, lastName: 'The Slayer' },
    });

    expect(wrapper.find('h3').text()).toBe('Frieren The Slayer');
  });

  it('links the name and the image to the character page', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props: baseProps });

    const links = wrapper.findAll('a');

    expect(links).toHaveLength(2);

    links.forEach((link) => {
      expect(link.attributes('href')).toBe(
        '/characters/7d2c4a9e-3b1f-4e6a-8c05-9a1e5f0b2d34',
      );
    });
  });

  it('renders the description when provided', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props: baseProps });

    expect(wrapper.text()).toContain(baseProps.description);

    expect(wrapper.text()).not.toContain('No description');
  });

  it('renders a fallback when the description is null', async () => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...baseProps, description: null },
    });

    expect(wrapper.text()).toContain('No description');
  });

  it('renders the age, gender and role', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props: baseProps });

    expect(wrapper.get('[data-testid="character-age"]').text()).toBe('1000+');

    expect(wrapper.get('[data-testid="character-gender"]').text()).toBe(
      'female',
    );

    expect(wrapper.get('[data-testid="character-role"]').text()).toBe('Main');
  });

  it('renders a fallback when the age is null', async () => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...baseProps, age: null },
    });

    expect(wrapper.get('[data-testid="character-age"]').text()).toBe('Unknown');
  });

  it.each([
    ['male', 'i-lucide:mars'],
    ['female', 'i-lucide:venus'],
    ['other', 'i-lucide:venus-and-mars'],
  ])('uses the correct gender icon for %s', async (gender, icon) => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...baseProps, gender },
    });

    expect(wrapper.get('[data-testid="character-gender"]').html()).toContain(
      icon,
    );
  });

  it('passes the name and image source to AppImage', async () => {
    const wrapper = await mountSuspended(CharacterCard, { props: baseProps });

    const image = wrapper.findComponent(AppImage);

    expect(image.exists()).toBe(true);

    expect(image.props('alt').trim()).toBe('Frieren');

    expect(image.props('src')).toBe('7d2c4a9e-3b1f-4e6a-8c05-9a1e5f0b2d34.jpg');
  });

  it('passes a null source to AppImage', async () => {
    const wrapper = await mountSuspended(CharacterCard, {
      props: { ...baseProps, imageSource: null },
    });

    expect(wrapper.findComponent(AppImage).props('src')).toBeNull();
  });
});
