<script lang="ts" setup>
defineProps<{
  title: string;

  image: Image;

  badges: Badge[];

  to: string;
}>();
</script>

<template>
  <Motion
    :transition="{ duration: 0.25 }"
    :while-hover="{ scale: 1 + 1 / 64 }"
    as-child
  >
    <UCard
      :ui="{
        root: 'relative flex flex-col',
        header: 'flex flex-1 items-center justify-center',
        body: 'flex justify-center',
      }"
      class="cursor-pointer"
    >
      <template #header>
        <h3 :title="title" class="font-bold text-xl text-center line-clamp-2">
          <NuxtLink :to="to" class="after:absolute after:inset-0 after:z-10">
            {{ title }}
          </NuxtLink>
        </h3>
      </template>
      <AppImage v-bind="image" />
      <template #footer>
        <div class="flex flex-wrap gap-1">
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
    </UCard>
  </Motion>
</template>
