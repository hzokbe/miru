<script lang="ts" setup>
const { animes, loading, error } = useAnimes();
</script>

<template>
  <AnimesSectionSkeleton v-if="loading" />
  <AppSection v-else icon="i-lucide-trophy" title="Top animes">
    <UAlert
      v-if="error"
      color="error"
      icon="i-lucide-circle-alert"
      orientation="horizontal"
      title="Something went wrong while loading the anime list. Please try again."
      variant="subtle"
    />
    <UAlert
      v-else-if="animes.length === 0"
      color="neutral"
      icon="i-lucide-list-video"
      orientation="horizontal"
      title="No anime found"
      variant="subtle"
    />
    <UPageGrid v-else class="auto-rows-fr">
      <AnimeCard
        v-for="anime in animes"
        :id="anime.id"
        :key="anime.id"
        :episodes="anime.episodes"
        :image-source="anime.imageSource"
        :score="anime.score"
        :status="anime.status"
        :title="anime.title"
        :type="anime.type"
      />
    </UPageGrid>
  </AppSection>
</template>
