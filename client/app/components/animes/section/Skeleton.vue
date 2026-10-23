<template>
  <AppSection icon="i-lucide-trophy" title="Top animes">
    <UPageGrid class="auto-rows-fr">
      <AnimeCardSkeleton
        v-for="(_, index) in Array.from({ length: 9 })"
        :key="index"
      />
    </UPageGrid>
  </AppSection>
</template>
