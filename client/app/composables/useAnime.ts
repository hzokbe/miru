export const useAnime = (id: string) => {
  const api = useAPI();

  const {
    data: anime,
    error,
    status,
    refresh,
  } = useAsyncData(`anime-${id}`, () => api<AnimeResponse>(`/animes/${id}`), {
    default: () => null,
  });

  return {
    anime,
    error,
    status,
    refresh,
  };
};
