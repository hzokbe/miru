<script lang="ts" setup>
const route = useRoute();

const { manga } = useManga(route.params.id as string);

useSeoMeta({
  title: () => manga.value?.title ?? 'Manga Details',
  description: () => manga.value?.synopsis ?? 'Manga details.',
});
</script>

<template>
  <MangaSection />
</template>
