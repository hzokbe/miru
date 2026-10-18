<script lang="ts" setup>
defineProps<{
  id: string;

  title: string;

  synopsis: string | null;

  episodes: number | null;

  status: AnimeStatus;

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
      <div class="flex gap-2">
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
      </div>
    </template>
  </UCard>
</template>
