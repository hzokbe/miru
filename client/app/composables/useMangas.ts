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

  return {
    mangas,
    error,
    status,
    refresh,
  };
};
