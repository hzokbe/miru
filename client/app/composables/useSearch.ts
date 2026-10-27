export const useSearch = () => {
  const api = useAPI();

  const { query } = useQuery();

  const {
    data: search,
    error,
    status,
    refresh,
  } = useAsyncData(
    `search-${query.value}`,
    () => api<SearchResponse>(`/search/${query.value}`),
    {
      default: () => null,
    },
  );

  return {
    search,
    error,
    status,
    refresh,
  };
};
