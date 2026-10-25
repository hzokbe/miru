<script lang="ts" setup>
const route = useRoute();

const { anime, loading, error } = useAnime(route.params.id as string);

const badges = computed<Badge[]>(() => {
  if (!anime.value) {
    return [];
  }

  return [
    {
      id: 'anime-score',
      icon: 'i-lucide-star',
      text: formatScore(anime.value.score),
      color: 'warning' as const,
      variant: 'subtle' as const,
    },
    {
      id: 'anime-episodes',
      icon: 'i-lucide-list-video',
      text: formatEpisodes(anime.value.episodes),
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
    {
      id: 'anime-status',
      icon: 'i-lucide-info',
      text: formatStatus(anime.value.status),
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
    {
      id: 'anime-type',
      icon: 'i-lucide-tv',
      text: formatType(anime.value.type),
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
    {
      id: 'anime-aired',
      icon: 'i-lucide-calendar',
      text: `${formatDate(anime.value.airedFrom)} - ${formatDate(anime.value.airedTo)}`,
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
  ];
});
</script>

<template>
  <AnimeSectionSkeleton v-if="loading" />
  <AppSection v-else>
    <UAlert
      v-if="error"
      color="error"
      icon="i-lucide-circle-alert"
      orientation="horizontal"
      title="Something went wrong while loading the anime details. Please try again."
      variant="subtle"
    />
    <UAlert
      v-else-if="!anime"
      color="neutral"
      icon="i-lucide-list-video"
      orientation="horizontal"
      title="Anime not found"
      variant="subtle"
    />
    <template v-else>
      <AppPageCard
        :badges="badges"
        :description="anime.synopsis ?? 'No synopsis'"
        :image="{ alt: anime.title, src: anime.imageSource }"
        :title="anime.title"
      />
      <CharactersSection :characters="anime.characters" />
    </template>
  </AppSection>
</template>
