export const useAnime = (id: string) => {
  const api = useAPI();

  const {
    data: anime,
    error,
    status,
    refresh,
  } = useAsyncData(
    `anime-${id}`,
    async () => {
      try {
        return await api<AnimeResponse>(`/animes/${id}`);
      } catch (cause) {
        if ((cause as { statusCode?: number }).statusCode === 404) {
          return null;
        }

        throw cause;
      }
    },
    {
      default: () => null,
    },
  );

  const loading = computed(
    () => status.value === 'idle' || status.value === 'pending',
  );

  return {
    anime,
    error,
    status,
    refresh,
    loading,
  };
};
