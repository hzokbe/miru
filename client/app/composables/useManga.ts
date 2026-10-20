export const useManga = (id: string) => {
  const api = useAPI();

  const {
    data: manga,
    error,
    status,
    refresh,
  } = useAsyncData(`manga-${id}`, () => api<MangaResponse>(`/mangas/${id}`), {
    default: () => null,
  });

  return {
    manga,
    error,
    status,
    refresh,
  };
};
