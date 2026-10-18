<script lang="ts" setup>
import type { NavigationMenuItem } from '@nuxt/ui';

const route = useRoute();

const items = computed<NavigationMenuItem[]>(() => [
  {
    label: 'Animes',
    to: '/animes',
    icon: 'i-lucide-clapperboard',
    active: route.path.endsWith('/animes'),
  },
]);
</script>

<template>
  <UHeader
    :class="{ 'fixed w-full': route.path == '/' }"
    :ui="{ title: 'items-center gap-2' }"
  >
    <template #title>
      <UIcon class="size-5 text-primary" name="i-lucide-eye" />
      <h1>Miru</h1>
    </template>
    <UNavigationMenu :items="items" />
    <template #right>
      <UColorModeButton />
    </template>
    <template #body>
      <UNavigationMenu :items="items" orientation="vertical" />
    </template>
  </UHeader>
</template>
