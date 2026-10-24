<script lang="ts" setup>
const { mangas, loading, error } = useMangas();
</script>

<template>
  <MangasSectionSkeleton v-if="loading" />
  <AppSection v-else icon="i-lucide-trophy" title="Top mangas">
    <UAlert
      v-if="error"
      color="error"
      icon="i-lucide-circle-alert"
      orientation="horizontal"
      title="Something went wrong while loading the manga list. Please try again."
      variant="subtle"
    />
    <UAlert
      v-else-if="mangas.length === 0"
      color="neutral"
      icon="i-lucide-book"
      orientation="horizontal"
      title="No manga found"
      variant="subtle"
    />
    <UPageGrid v-else class="auto-rows-fr">
      <MangaCard
        v-for="manga in mangas"
        :id="manga.id"
        :key="manga.id"
        :chapters="manga.chapters"
        :image-source="manga.imageSource"
        :score="manga.score"
        :status="manga.status"
        :title="manga.title"
        :volumes="manga.volumes"
      />
    </UPageGrid>
  </AppSection>
</template>
