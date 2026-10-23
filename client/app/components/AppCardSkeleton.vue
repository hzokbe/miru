<script lang="ts" setup>
defineProps<{
  title: string;

  image: Image;
}>();
</script>

<template>
  <UCard
    :ui="{
      root: 'relative flex flex-col',
      header: 'flex flex-1 items-center justify-center',
      body: 'flex justify-center',
    }"
  >
    <template #header>
      <div class="relative">
        <h3 class="invisible font-bold text-xl text-center line-clamp-2">
          {{ title }}
        </h3>
        <USkeleton class="absolute inset-0" />
      </div>
    </template>
    <div class="relative">
      <AppImage class="invisible" v-bind="image" />
      <USkeleton class="absolute inset-0" />
    </div>
    <template #footer>
      <USkeleton class="h-6 md:h-13 w-full" />
    </template>
  </UCard>
</template>
