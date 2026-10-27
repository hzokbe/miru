<script lang="ts" setup>
import type { NavigationMenuItem } from '@nuxt/ui';

const route = useRoute();

const introPlayed = useState('header-intro-played', () => false);

onMounted(() => {
  introPlayed.value = true;
});

const items = computed<NavigationMenuItem[]>(() => [
  {
    label: 'Animes',
    to: '/animes',
    icon: 'i-lucide-clapperboard',
    active: route.path.endsWith('/animes'),
  },
  {
    label: 'Mangas',
    to: '/mangas',
    icon: 'i-lucide-book',
    active: route.path.endsWith('/mangas'),
  },
]);

const transition = {
  duration: 0.75,
  ease: 'easeOut',
  delay: 0.25,
} as const;
</script>

<template>
  <UHeader
    :class="{
      'fixed w-full': route.path == '/',
      'app-header-background': true,
    }"
    :ui="{ title: 'items-center gap-2', right: 'flex-1' }"
  >
    <template #title>
      <Motion
        :animate="{ opacity: 1 }"
        :initial="introPlayed ? false : { opacity: 0 }"
        :transition="transition"
      >
        <h1
          class="bg-linear-to-r from-primary-400 via-primary-300 to-primary-500 bg-clip-text text-transparent hover:opacity-85 transition-all duration-200"
        >
          Miru
        </h1>
      </Motion>
    </template>
    <Motion
      :animate="{ opacity: 1 }"
      :initial="introPlayed ? false : { opacity: 0 }"
      :transition="transition"
    >
      <UNavigationMenu :items="items" />
    </Motion>
    <template #right>
      <Motion
        :animate="{ opacity: 1 }"
        :initial="introPlayed ? false : { opacity: 0 }"
        :transition="transition"
        class="flex items-center gap-2 w-full justify-end"
      >
        <SearchDrawer />
        <UColorModeButton />
      </Motion>
    </template>
    <template #toggle="{ open, toggle, ui }">
      <Motion
        :animate="{ opacity: 1 }"
        :initial="introPlayed ? false : { opacity: 0 }"
        :transition="transition"
      >
        <UButton
          :class="ui.toggle()"
          :icon="open ? 'i-lucide-x' : 'i-lucide-menu'"
          aria-label="Toggle menu"
          color="neutral"
          variant="ghost"
          @click="toggle"
        />
      </Motion>
    </template>
    <template #body>
      <UNavigationMenu :items="items" orientation="vertical" />
    </template>
  </UHeader>
</template>
