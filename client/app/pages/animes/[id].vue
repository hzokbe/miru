<script lang="ts" setup>
const route = useRoute();

const { anime, status, refresh } = useAnime(route.params.id as string);

useSeoMeta({
  title: () => anime.value?.title ?? 'Anime Details',
  description: () => anime.value?.synopsis ?? 'Anime details.',
});

const isLoading = computed(
  () => status.value === 'idle' || status.value === 'pending',
);

const showLoading = import.meta.client ? useDelay(isLoading) : ref(false);

const formatStatus = (value: string): string =>
  value.replace(/([a-z])([A-Z])/g, '$1 $2');

const formatDate = (value: string | null): string => {
  if (!value) {
    return '?';
  }

  const date = new Date(value);

  if (Number.isNaN(date.getTime())) {
    return '?';
  }

  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: '2-digit',
    timeZone: 'UTC',
  });
};
</script>

<template>
  <UPageSection v-if="status !== 'success'">
    <UProgress v-if="showLoading" animation="swing" color="neutral" />
    <UEmpty
      v-else-if="status === 'error'"
      :actions="[
        {
          label: 'Try again',
          icon: 'i-lucide-refresh-cw',
          onClick: () => refresh(),
        },
      ]"
      description="Something went wrong while fetching the anime details."
      icon="i-lucide-circle-x"
      title="Unable to load anime"
    />
  </UPageSection>
  <UPageSection v-else-if="status === 'success' && anime">
    <template #header>
      <div class="flex flex-col items-center justify-center gap-4">
        <AppImage :alt="anime.title" :src="anime.imageSource" size="md" />
        <div>
          <h2
            class="text-3xl sm:text-4xl lg:text-5xl text-pretty tracking-tight font-bold text-highlighted text-center"
          >
            {{ anime.title }}
          </h2>
          <h3
            v-if="anime.japaneseTitle"
            class="text-base sm:text-lg text-muted text-center text-balance mt-6"
          >
            {{ anime.japaneseTitle }}
          </h3>
        </div>
      </div>
    </template>
    <UCard>
      <template #header>
        <p class="text-base sm:text-lg text-muted">
          {{ anime.synopsis ?? 'No synopsis' }}
        </p>
      </template>
      <template #footer>
        <div class="flex flex-wrap gap-2">
          <UBadge
            color="neutral"
            data-testid="anime-score"
            icon="i-lucide-star"
            variant="outline"
          >
            {{ anime.score ?? 0.0 }}
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="anime-type"
            icon="i-lucide-tv"
            variant="outline"
          >
            {{ formatAnimeType(anime.type) }}
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="anime-episodes"
            icon="i-lucide-list-video"
            variant="outline"
          >
            {{ anime.episodes ?? 0 }} episodes
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="anime-status"
            icon="i-lucide-info"
            variant="outline"
          >
            {{ formatStatus(anime.status) }}
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="anime-aired"
            icon="i-lucide-calendar"
            variant="outline"
          >
            {{ formatDate(anime.airedFrom) }} -
            {{ formatDate(anime.airedTo) }}
          </UBadge>
        </div>
      </template>
    </UCard>
  </UPageSection>
</template>
