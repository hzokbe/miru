export const useMangas = () => {
  const api = useAPI();

  const {
    data: mangas,
    error,
    status,
    refresh,
  } = useAsyncData('mangas-all', () => api<MangaResponse[]>('/mangas'), {
    default: () => [],
  });

  const loading = computed(
    () => status.value === 'idle' || status.value === 'pending',
  );

  return {
    mangas,
    error,
    status,
    refresh,
    loading,
  };
};
