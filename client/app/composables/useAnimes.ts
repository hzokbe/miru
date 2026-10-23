export const useAnimes = () => {
  const api = useAPI();

  const {
    data: animes,
    error,
    status,
    refresh,
  } = useAsyncData('animes-all', () => api<AnimeResponse[]>('/animes'), {
    default: () => [],
  });

  const loading = computed(
    () => status.value === 'idle' || status.value === 'pending',
  );

  return {
    animes,
    error,
    status,
    refresh,
    loading,
  };
};
