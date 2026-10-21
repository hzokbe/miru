<script lang="ts" setup>
const route = useRoute();

const { manga, status, refresh } = useManga(route.params.id as string);

useSeoMeta({
  title: () => manga.value?.title ?? 'Manga Details',
  description: () => manga.value?.synopsis ?? 'Manga details.',
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
      description="Something went wrong while fetching the manga details."
      icon="i-lucide-circle-x"
      title="Unable to load manga"
    />
  </UPageSection>
  <UPageSection v-else-if="status === 'success' && manga">
    <template #header>
      <div class="flex flex-col items-center justify-center gap-4">
        <AppImage :alt="manga.title" :src="manga.imageSource" size="md" />
        <div>
          <h2
            class="text-3xl sm:text-4xl lg:text-5xl text-pretty tracking-tight font-bold text-highlighted text-center"
          >
            {{ manga.title }}
          </h2>
          <h3
            v-if="manga.japaneseTitle"
            class="text-base sm:text-lg text-muted text-center text-balance mt-6"
          >
            {{ manga.japaneseTitle }}
          </h3>
        </div>
      </div>
    </template>
    <UCard>
      <template #header>
        <p class="text-base sm:text-lg text-muted">
          {{ manga.synopsis ?? 'No synopsis' }}
        </p>
      </template>
      <template #footer>
        <div class="flex flex-wrap gap-2">
          <UBadge
            color="neutral"
            data-testid="manga-score"
            icon="i-lucide-star"
            variant="outline"
          >
            {{ manga.score ?? 0.0 }}
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="manga-chapters"
            icon="i-lucide-book-open"
            variant="outline"
          >
            {{ manga.chapters ?? 0 }} chapters
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="manga-volumes"
            icon="i-lucide-library"
            variant="outline"
          >
            {{ manga.volumes ?? 0 }} volumes
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="manga-status"
            icon="i-lucide-info"
            variant="outline"
          >
            {{ formatStatus(manga.status) }}
          </UBadge>
          <UBadge
            color="neutral"
            data-testid="manga-published"
            icon="i-lucide-calendar"
            variant="outline"
          >
            {{ formatDate(manga.publishedFrom) }} -
            {{ formatDate(manga.publishedTo) }}
          </UBadge>
          <template v-if="manga.genres">
            <UBadge
              v-for="genre in manga.genres"
              :key="genre.id"
              color="neutral"
              data-testid="manga-genre"
              icon="i-lucide-drama"
              variant="outline"
            >
              {{ genre.name }}
            </UBadge>
          </template>
        </div>
      </template>
    </UCard>
  </UPageSection>
</template>
