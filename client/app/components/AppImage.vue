<script lang="ts" setup>
defineProps<{
  alt: string;

  src: string | null;
}>();
</script>

<template>
  <img
    v-if="src"
    :alt="alt"
    :src="src"
    class="aspect-2/3 w-48 rounded-sm object-cover ring ring-default"
    loading="lazy"
  />
  <div
    v-else
    class="flex aspect-2/3 w-48 items-center justify-center rounded-sm ring ring-default"
    data-testid="image-fallback"
  >
    <UIcon name="i-lucide-image-off" />
  </div>
</template>
