<script lang="ts" setup>
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';

export interface MangaCardProps {
  id: string;

  title: string;

  chapters: number | null;

  volumes: number | null;

  status: MangaStatus;

  score: number | null;

  imageSource: string | null;
}

const props = defineProps<MangaCardProps>();

const badges: Badge[] = [
  {
    id: 'manga-score',
    text: formatScore(props.score),
    color: 'warning',
    icon: 'i-lucide-star',
  },
  {
    id: 'manga-volumes',
    text: formatVolumes(props.volumes),
    color: 'neutral',
    icon: 'i-lucide-book',
  },
  {
    id: 'manga-chapters',
    text: formatChapters(props.chapters),
    color: 'neutral',
    icon: 'i-lucide-library',
  },
  {
    id: 'manga-status',
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
    :to="`/mangas/${props.id}`"
  />
</template>
