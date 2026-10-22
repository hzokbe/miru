<script lang="ts" setup>
useSeoMeta({
  title: 'Miru - Track your anime and manga',
  description:
    'Keep track of everything you watch and read. Build your lists, rate your favorites, and discover your next obsession.',
});
</script>

<template>
  <Motion
    :animate="{ opacity: 1 }"
    :initial="{ opacity: 0 }"
    :transition="{
      duration: 0.75,
      ease: 'easeOut',
    }"
  >
    <UPageHero
      class="flex flex-1 items-center min-h-dvh min-w-dvw"
      description="Keep track of everything you watch and read. Build your lists, rate your favorites, and discover your next obsession."
      title="Your anime and manga, all in one place."
    />
  </Motion>
</template>
