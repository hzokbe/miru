<script lang="ts" setup>
const { search, status, refresh } = useSearch();

const { clearQuery } = useQuery();

const router = useRouter();

const stop = router.afterEach((to) => {
  if (to.path !== '/search') {
    clearQuery();

    stop();
  }
});

onBeforeUnmount(stop);
</script>

<template>
  <AsyncState
    :status="status"
    error-description="Something went wrong while searching."
    error-title="Unable to search"
    @retry="refresh()"
  >
    <AppEmpty
      v-if="!search"
      icon="i-lucide-list-x"
      title="No animes, mangas or characters found."
    />
    <UContainer v-else class="py-8">
      <UPageBody>
        <section v-if="search.animes.length">
          <h2 class="mb-6 text-2xl font-semibold">Animes</h2>
          <UPageGrid>
            <AnimeCard
              v-for="anime in search.animes"
              :key="anime.id"
              v-bind="anime"
            />
          </UPageGrid>
        </section>
      </UPageBody>
      <UPageBody>
        <section v-if="search.mangas.length">
          <h2 class="mb-6 text-2xl font-semibold">Mangas</h2>
          <UPageGrid>
            <MangaCard
              v-for="manga in search.mangas"
              :key="manga.id"
              v-bind="manga"
            />
          </UPageGrid>
        </section>
      </UPageBody>
      <UPageBody>
        <section v-if="search.characters.length">
          <h2 class="mb-6 text-2xl font-semibold">Characters</h2>
          <UPageGrid>
            <CharacterCard
              v-for="character in search.characters"
              :key="character.id"
              v-bind="character"
            />
          </UPageGrid>
        </section>
      </UPageBody>
    </UContainer>
  </AsyncState>
</template>
