<script lang="ts" setup>
defineProps<{
  id: string;

  title: string;

  synopsis: string | null;

  chapters: number | null;

  volumes: number | null;

  status: MangaStatus;

  score: number | null;

  imageSource: string | null;
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
        :to="`/mangas/${id}`"
        class="hover:brightness-75 transition-all duration-200"
      >
        <h3 class="font-bold">{{ title }}</h3>
      </NuxtLink>
      <p class="text-muted line-clamp-2">{{ synopsis ?? 'No synopsis' }}</p>
    </template>
    <NuxtLink
      :to="`/mangas/${id}`"
      class="hover:brightness-75 transition-all duration-200"
    >
      <AppImage :alt="title" :src="imageSource" />
    </NuxtLink>
    <template #footer>
      <div class="flex flex-wrap gap-2">
        <UBadge
          class="whitespace-nowrap"
          color="neutral"
          data-testid="manga-score"
          icon="i-lucide-star"
          variant="outline"
        >
          {{ score ?? 0.0 }}
        </UBadge>
        <UBadge
          class="whitespace-nowrap"
          color="neutral"
          data-testid="manga-chapters"
          icon="i-lucide-book-open"
          variant="outline"
        >
          {{ chapters ?? 0 }} chapters
        </UBadge>
        <UBadge
          class="whitespace-nowrap"
          color="neutral"
          data-testid="manga-volumes"
          icon="i-lucide-library"
          variant="outline"
        >
          {{ volumes ?? 0 }} volumes
        </UBadge>
        <UBadge
          class="whitespace-nowrap"
          color="neutral"
          data-testid="manga-status"
          icon="i-lucide-info"
          variant="outline"
        >
          {{ formatStatus(status) }}
        </UBadge>
      </div>
    </template>
  </UCard>
</template>
