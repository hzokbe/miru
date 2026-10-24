<script lang="ts" setup>
defineProps<{
  title: string;

  description: string;

  image: Image;
}>();
</script>

<template>
  <Motion
    :transition="{ duration: 0.25 }"
    :while-hover="{ scale: 1 + 1 / 64 }"
    as-child
  >
    <UPageCard
      :ui="{
        container: 'lg:grid lg:grid-cols-[auto_1fr] gap-x-6 items-center',
        wrapper: 'items-start',
        footer: 'w-full',
      }"
      orientation="horizontal"
      reverse
      spotlight
    >
      <template #title>
        <div class="relative">
          <h3 class="invisible font-bold text-xl line-clamp-2">
            {{ title }}
          </h3>
          <USkeleton class="absolute inset-0" />
        </div>
      </template>
      <template #description>
        <div class="relative">
          <p class="invisible text-muted">
            {{ description }}
          </p>
          <USkeleton class="absolute inset-0" />
        </div>
      </template>
      <div class="relative">
        <AppImage class="invisible" v-bind="image" />
        <USkeleton class="absolute inset-0" />
      </div>
      <template #footer>
        <USkeleton class="h-13 sm:h-6 w-full" />
      </template>
    </UPageCard>
  </Motion>
</template>
