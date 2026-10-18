<script lang="ts" setup>
useSeoMeta({
  title: 'Miru - Animes',
});

const { animes, status, refresh } = useAnimes();

const isLoading = computed(
  () => status.value === 'idle' || status.value === 'pending',
);

const showLoading = import.meta.client ? useDelay(isLoading) : ref(false);
</script>

<template>
  <UPageSection title="Top animes">
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
      title="Unable to load animes"
    />
    <UPageGrid v-else-if="status === 'success'">
      <AnimeCard
        v-for="anime in animes"
        :id="anime.id"
        :key="anime.id"
        :episodes="anime.episodes"
        :image-source="anime.imageSource"
        :score="anime.score"
        :status="anime.status"
        :synopsis="anime.synopsis"
        :title="anime.title"
      />
    </UPageGrid>
  </UPageSection>
</template>
