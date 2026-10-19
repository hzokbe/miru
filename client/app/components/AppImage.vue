<script lang="ts" setup>
withDefaults(
  defineProps<{
    alt: string;
    src: string | null;
    size?: 'sm' | 'md';
  }>(),
  {
    size: 'sm',
  },
);
</script>

<template>
  <img
    v-if="src"
    :alt="alt"
    :class="`${size == 'sm' ? 'w-32' : 'w-48'} aspect-2/3 rounded-sm ring ring-default`"
    :src="src"
    loading="lazy"
  />
  <div
    v-else
    :class="`${size == 'sm' ? 'w-32' : 'w-48'} aspect-2/3 rounded-sm ring ring-default flex items-center justify-center`"
  >
    <UIcon name="i-lucide-image-off" />
  </div>
</template>
