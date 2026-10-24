<template>
  <AppSection icon="i-lucide-trophy" title="Top mangas">
    <UPageGrid class="auto-rows-fr">
      <MangaCardSkeleton
        v-for="(_, index) in Array.from({ length: 9 })"
        :key="index"
      />
    </UPageGrid>
  </AppSection>
</template>
