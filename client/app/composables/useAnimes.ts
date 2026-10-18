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

  return {
    animes,
    error,
    status,
    refresh,
  };
};
