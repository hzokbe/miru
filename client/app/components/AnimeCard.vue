<script lang="ts" setup>
import type { GenreResponse } from '~/types/genre-response.ts';

defineProps<{
  id: string;

  title: string;

  synopsis: string | null;

  episodes: number | null;

  status: AnimeStatus;

  score: number | null;

  imageSource: string | null;

  genres: GenreResponse[];
}>();

const formatStatus = (value: string): string =>
  value.replace(/([a-z])([A-Z])/g, '$1 $2');
</script>

<template>
  <UCard
    :ui="{
      root: 'flex flex-col',
      header: 'flex-1',
      body: 'flex justify-center',
    }"
  >
    <template #header>
      <NuxtLink
        :to="`/animes/${id}`"
        class="hover:brightness-75 transition-all duration-200"
      >
        <h3 class="font-bold">{{ title }}</h3>
      </NuxtLink>
      <p class="text-muted line-clamp-2">{{ synopsis ?? 'No synopsis' }}</p>
    </template>
    <NuxtLink
      :to="`/animes/${id}`"
      class="hover:brightness-75 transition-all duration-200"
    >
      <AppImage :alt="title" :src="imageSource" />
    </NuxtLink>
    <template #footer>
      <div class="flex flex-wrap gap-2">
        <UBadge
          color="neutral"
          data-testid="anime-score"
          icon="i-lucide-star"
          variant="outline"
        >
          {{ score ?? 0.0 }}
        </UBadge>
        <UBadge
          color="neutral"
          data-testid="anime-episodes"
          icon="i-lucide-list-video"
          variant="outline"
        >
          {{ episodes ?? 0 }} episodes
        </UBadge>
        <UBadge
          color="neutral"
          data-testid="anime-status"
          icon="i-lucide-info"
          variant="outline"
        >
          {{ formatStatus(status) }}
        </UBadge>
        <template v-if="genres && genres.length <= 2">
          <UBadge
            v-for="genre in genres"
            :key="genre.id"
            color="neutral"
            data-testid="anime-genre"
            icon="i-lucide-drama"
            variant="outline"
          >
            {{ genre.name }}
          </UBadge>
        </template>
        <template v-if="genres && genres.length > 2">
          <UBadge
            v-for="genre in genres.slice(0, 2)"
            :key="genre.id"
            color="neutral"
            data-testid="anime-genre"
            icon="i-lucide-drama"
            variant="outline"
          >
            {{ genre.name }}
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="anime-genres-overflow"
            icon="i-lucide-drama"
            variant="outline"
          >
            + {{ genres.length - 2 }}
          </UBadge>
        </template>
      </div>
    </template>
  </UCard>
</template>
