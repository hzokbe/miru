<script lang="ts" setup>
defineProps<{
  title: string;

  description: string;

  image: Image;

  badges: Badge[];
}>();
</script>

<template>
  <Motion
    :transition="{ duration: 0.25 }"
    :while-hover="{ scale: 1 + 1 / 64 }"
    as-child
  >
    <UPageCard
      :description="description"
      :title="title"
      :ui="{
        container: 'lg:grid lg:grid-cols-[auto_1fr] gap-x-6 items-center',
      }"
      orientation="horizontal"
      reverse
      spotlight
    >
      <template #title>
        <h3 class="font-bold text-xl line-clamp-2">
          {{ title }}
        </h3>
      </template>
      <template #description>
        <p class="text-muted">
          {{ description }}
        </p>
      </template>
      <AppImage v-bind="image" />
      <template #footer>
        <div class="flex flex-wrap gap-1 w-full">
          <UBadge
            v-for="badge in badges"
            :key="badge.id"
            :color="badge.color"
            :data-testid="badge.id"
            :icon="badge.icon"
            size="md"
            variant="subtle"
          >
            {{ badge.text }}
          </UBadge>
        </div>
      </template>
    </UPageCard>
  </Motion>
</template>
