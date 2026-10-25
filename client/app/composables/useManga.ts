export const useManga = (id: string) => {
  const api = useAPI();

  const {
    data: manga,
    error,
    status,
    refresh,
  } = useAsyncData(
    `manga-${id}`,
    async () => {
      try {
        return await api<MangaResponse>(`/mangas/${id}`);
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
    manga,
    error,
    status,
    refresh,
    loading,
  };
};
