<script lang="ts" setup>
import { fullName } from '~/utils/character-utils';

const api = useAPI();

const { query, clearQuery } = useQuery();

const open = ref(false);

const debouncedQuery = refDebounced(query, 300);

const { data, status } = useLazyAsyncData(
  'search-suggestions',
  async () => {
    const term = debouncedQuery.value.trim();

    if (!term) {
      return null;
    }

    return await api<SearchResponse>(`/search/${term}`);
  },
  {
    watch: [debouncedQuery],
    default: () => null,
  },
);

const limit = 5;

const groups = computed(() => {
  const result = data.value;

  return [
    {
      id: 'animes',
      label: 'Animes',
      ignoreFilter: true,
      items: (result?.animes ?? []).slice(0, limit).map((anime) => ({
        label: anime.title,
        icon: 'i-lucide-clapperboard',
        to: `/animes/${anime.id}`,
      })),
    },
    {
      id: 'mangas',
      label: 'Mangas',
      ignoreFilter: true,
      items: (result?.mangas ?? []).slice(0, limit).map((manga) => ({
        label: manga.title,
        icon: 'i-lucide-book',
        to: `/mangas/${manga.id}`,
      })),
    },
    {
      id: 'characters',
      label: 'Characters',
      ignoreFilter: true,
      items: (result?.characters ?? []).slice(0, limit).map((character) => ({
        label: fullName(character.name, character.lastName),
        icon: 'i-lucide-user',
        to: `/characters/${character.id}`,
      })),
    },
  ].filter((group) => group.items.length);
});

const onSelect = () => {
  open.value = false;
};

const onSubmit = async () => {
  const term = query.value.trim();

  if (!term) {
    return;
  }

  open.value = false;

  await navigateTo('/search/' + term);
};

watch(open, (value) => {
  if (!value) {
    clearQuery();
  }
});

defineShortcuts({
  meta_k: () => (open.value = !open.value),
});
</script>

<template>
  <UDrawer v-model:open="open" :handle="false" direction="top" inset>
    <UButton color="neutral" icon="i-lucide-search" variant="ghost">
      <template #trailing>
        <UKbd value="meta" />
        <UKbd value="K" />
      </template>
    </UButton>
    <template #content>
      <div @keydown.enter="onSubmit">
        <UCommandPalette
          v-model:search-term="query"
          :groups="groups"
          :loading="status === 'pending'"
          placeholder="Search animes, mangas or characters"
          @update:model-value="onSelect"
        />
      </div>
    </template>
  </UDrawer>
</template>
