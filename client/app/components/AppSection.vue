<script lang="ts" setup>
defineProps<{
  icon: string;

  title: string;
}>();
</script>

<template>
  <Motion
    :animate="{
      opacity: 1,
    }"
    :initial="{
      opacity: 0,
    }"
    :transition="{
      duration: 1,
      ease: 'easeOut',
    }"
  >
    <UPageSection
      :icon="icon"
      :title="title"
      class="flex flex-1 items-center justify-center"
    >
      <slot />
    </UPageSection>
  </Motion>
</template>
