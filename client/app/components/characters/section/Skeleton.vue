<template>
  <AppSection icon="i-lucide-user" title="Characters">
    <UPageGrid class="auto-rows-fr">
      <CharacterCardSkeleton
        v-for="(_, index) in Array.from({ length: 9 })"
        :key="index"
      />
    </UPageGrid>
  </AppSection>
</template>
