<script lang="ts" setup>
export interface AnimeCardProps {
  id: string;

  title: string;

  type: AnimeType;

  episodes: number | null;

  status: AnimeStatus;

  score: number | null;

  imageSource: string | null;
}

const props = defineProps<AnimeCardProps>();

const badges: Badge[] = [
  {
    id: 'anime-score',
    text: formatScore(props.score),
    color: 'warning',
    icon: 'i-lucide-star',
  },
  {
    id: 'anime-type',
    text: formatType(props.type),
    color: 'neutral',
    icon: 'i-lucide-tag',
  },
  {
    id: 'anime-episodes',
    text: formatEpisodes(props.episodes),
    color: 'neutral',
    icon: 'i-lucide-list-video',
  },
  {
    id: 'anime-status',
    text: formatStatus(props.status),
    color: 'neutral',
    icon: 'i-lucide-info',
  },
];
</script>

<template>
  <AppCard
    :badges="badges"
    :image="{ alt: props.title, src: props.imageSource }"
    :title="title"
    :to="`/animes/${props.id}`"
  />
</template>
