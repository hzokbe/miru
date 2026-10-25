<script lang="ts" setup>
defineProps<{
  characters: CharacterResponse[];
}>();
</script>

<template>
  <AppSection icon="i-lucide-user" title="Characters">
    <UPageGrid class="auto-rows-fr">
      <CharacterCard
        v-for="character in characters"
        :id="character.id"
        :key="character.id"
        :age="character.age"
        :description="character.description"
        :gender="character.gender"
        :image-source="character.imageSource"
        :last-name="character.lastName"
        :name="character.name"
        :role="character.role"
      />
    </UPageGrid>
  </AppSection>
</template>
