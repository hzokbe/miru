<template>
  <AppCardSkeleton
    :badges="[
      {
        id: 'anime-score',
        text: formatScore(8.4),
        color: 'neutral',
        icon: 'i-lucide-star',
      },
      {
        id: 'anime-type',
        text: formatType('Tv'),
        color: 'neutral',
        icon: 'i-lucide-tag',
      },
      {
        id: 'anime-episodes',
        text: formatEpisodes(12),
        color: 'neutral',
        icon: 'i-lucide-list-video',
      },
      {
        id: 'anime-status',
        text: formatStatus('FinishedAiring'),
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
