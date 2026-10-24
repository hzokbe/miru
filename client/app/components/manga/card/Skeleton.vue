<script lang="ts" setup>
import { formatScore, formatStatus } from '~/utils/utils.ts';
import { formatChapters, formatVolumes } from '~/utils/manga-utils.ts';
</script>

<template>
  <AppCardSkeleton
    :badges="[
      {
        id: 'manga-score',
        text: formatScore(8.88),
        color: 'neutral',
        icon: 'i-lucide-star',
      },
      {
        id: 'manga-volumes',
        text: formatVolumes(28),
        color: 'neutral',
        icon: 'i-lucide-book',
      },
      {
        id: 'manga-chapters',
        text: formatChapters(281),
        color: 'neutral',
        icon: 'i-lucide-library',
      },
      {
        id: 'manga-status',
        text: formatStatus('Finished'),
        color: 'neutral',
        icon: 'i-lucide-info',
      },
    ]"
    :image="{
      alt: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
      src: null,
    }"
    title="Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen"
  />
</template>
