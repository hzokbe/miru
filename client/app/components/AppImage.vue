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
    class="w-32 aspect-2/3 rounded-sm ring ring-default"
    loading="lazy"
  />
  <div
    v-else
    class="w-32 aspect-2/3 rounded-sm ring ring-default flex items-center justify-center"
  >
    <UIcon name="i-lucide-image-off" />
  </div>
</template>
