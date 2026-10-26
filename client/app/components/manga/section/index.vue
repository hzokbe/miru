<script lang="ts" setup>
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';

const route = useRoute();

const { manga, loading, error } = useManga(route.params.id as string);

const badges = computed<Badge[]>(() => {
  if (!manga.value) {
    return [];
  }

  return [
    {
      id: 'manga-score',
      icon: 'i-lucide-star',
      text: formatScore(manga.value.score),
      color: 'warning' as const,
      variant: 'subtle' as const,
    },
    {
      id: 'manga-chapters',
      icon: 'i-lucide-book',
      text: formatChapters(manga.value.chapters),
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
    {
      id: 'manga-volumes',
      icon: 'i-lucide-library',
      text: formatVolumes(manga.value.volumes),
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
    {
      id: 'manga-status',
      icon: 'i-lucide-info',
      text: formatStatus(manga.value.status),
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
    {
      id: 'manga-published',
      icon: 'i-lucide-calendar',
      text: `${formatDate(manga.value.publishedFrom)} - ${formatDate(manga.value.publishedTo)}`,
      color: 'neutral' as const,
      variant: 'outline' as const,
    },
  ];
});
</script>

<template>
  <MangaSectionSkeleton v-if="loading" />
  <AppSection v-else>
    <UAlert
      v-if="error"
      color="error"
      icon="i-lucide-circle-alert"
      orientation="horizontal"
      title="Something went wrong while loading the manga details. Please try again."
      variant="subtle"
    />
    <UAlert
      v-else-if="!manga"
      color="neutral"
      icon="i-lucide-list-video"
      orientation="horizontal"
      title="Manga not found"
      variant="subtle"
    />

    <template v-else>
      <AppPageCard
        :badges="badges"
        :description="manga.synopsis ?? 'No synopsis'"
        :image="{ alt: manga.title, src: manga.imageSource }"
        :title="manga.title"
      />
      <CharactersSection :characters="manga.characters" />
    </template>
  </AppSection>
</template>
