<script lang="ts" setup>
import { fullName } from '~/utils/character-utils.ts';

const route = useRoute();

const { character } = useCharacter(route.params.id as string);

useSeoMeta({
  title: () => {
    if (!character.value) {
      return 'Character Details';
    }

    return fullName(character.value.name, character.value.lastName);
  },
  description: () => character.value?.description ?? 'Character details.',
});
</script>

<template>
  <CharacterSection />
</template>
