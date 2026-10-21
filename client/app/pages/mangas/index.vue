<script lang="ts" setup>
useSeoMeta({
  title: 'Miru - Mangas',
});

const { mangas, status, refresh } = useMangas();

const isLoading = computed(
  () => status.value === 'idle' || status.value === 'pending',
);

const showLoading = import.meta.client ? useDelay(isLoading) : ref(false);
</script>

<template>
  <UPageSection title="Top mangas">
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
      description="Something went wrong while fetching the list."
      icon="i-lucide-circle-x"
      title="Unable to load mangas"
    />
    <UPageGrid v-else-if="status === 'success'">
      <MangaCard
        v-for="manga in mangas"
        :id="manga.id"
        :key="manga.id"
        :chapters="manga.chapters"
        :genres="manga.genres"
        :image-source="manga.imageSource"
        :score="manga.score"
        :status="manga.status"
        :synopsis="manga.synopsis"
        :title="manga.title"
        :volumes="manga.volumes"
      />
    </UPageGrid>
  </UPageSection>
</template>
