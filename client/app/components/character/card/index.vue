<script lang="ts" setup>
import type { CharacterGender } from '~/types/character-response.ts';
import { fullName, getGenderIcon } from '~/utils/character-utils.ts';

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
</script>

<template>
  <AppCard
    :badges="badges"
    :image="{ alt: fullName(name, lastName), src: props.imageSource }"
    :title="fullName(name, lastName)"
    :to="`/characters/${props.id}`"
  />
</template>
