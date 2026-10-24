<script lang="ts" setup>
const route = useRoute();

const { anime } = useAnime(route.params.id as string);

useSeoMeta({
  title: () => anime.value?.title ?? 'Anime Details',
  description: () => anime.value?.synopsis ?? 'Anime details.',
});
</script>

<template>
  <AnimeSection :anime="anime" />
</template>
