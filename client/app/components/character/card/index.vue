<script lang="ts" setup>
import type { CharacterGender } from '~/types/character-response.ts';

export interface AnimeCardProps {
  id: string;

  name: string;

  lastName: string | null;

  age: string | null;

  gender: CharacterGender;

  role: string;

  description: string | null;

  imageSource: string | null;
}

const props = defineProps<AnimeCardProps>();

const gender = props.gender;

const getGenderIcon = (gender: CharacterGender) => {
  switch (gender) {
    case 'Female':
      return 'i-lucide-venus';
    case 'Male':
      return 'i-lucide-mars';
    default:
      return 'i-lucide-venus-mars';
  }
};

const badges: Badge[] = [
  {
    id: 'character-role',
    text: props.role,
    color: 'neutral',
    icon: 'i-lucide-tag',
  },
  {
    id: 'character-gender',
    text: gender,
    color: 'neutral',
    icon: getGenderIcon(gender),
  },
  {
    id: 'character-age',
    text: props.age ?? '?',
    color: 'neutral',
    icon: 'i-lucide-cake',
  },
];

const fullName = computed(() =>
  [props.name, props.lastName].filter(Boolean).join(' '),
);
</script>

<template>
  <AppCard
    :badges="badges"
    :image="{ alt: fullName, src: props.imageSource }"
    :title="fullName"
    :to="`/characters/${props.id}`"
  />
</template>
