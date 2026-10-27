<script lang="ts" setup>
import { fullName, getGenderIcon } from '~/utils/character-utils.ts';

const route = useRoute();

const { character, loading, error } = useCharacter(route.params.id as string);

const badges = computed<Badge[]>(() => {
  if (!character.value) {
    return [];
  }

  const gender = character.value.gender;

  return [
    {
      id: 'character-role',
      text: character.value.role,
      color: 'neutral',
      icon: 'i-lucide-tag',
    },
    {
      id: 'character-gender',
      text: character.value.gender,
      color: 'neutral',
      icon: getGenderIcon(gender),
    },
    {
      id: 'character-age',
      text: character.value.age ?? '?',
      color: 'neutral',
      icon: 'i-lucide-cake',
    },
  ];
});
</script>

<template>
  <CharacterSectionSkeleton v-if="loading" />
  <AppSection v-else>
    <UAlert
      v-if="error"
      color="error"
      icon="i-lucide-circle-alert"
      orientation="horizontal"
      title="Something went wrong while loading the character details. Please try again."
      variant="subtle"
    />
    <UAlert
      v-else-if="!character"
      color="neutral"
      icon="i-lucide-user"
      orientation="horizontal"
      title="Character not found"
      variant="subtle"
    />
    <template v-else>
      <AppPageCard
        :badges="badges"
        :description="character.description ?? 'No description'"
        :image="{
          alt: fullName(character.name, character.lastName),
          src: character.imageSource,
        }"
        :title="fullName(character.name, character.lastName)"
      />
    </template>
  </AppSection>
</template>
