export const useQuery = () => {
  const query = useState('query', () => '');

  const clearQuery = () => (query.value = '');

  const setQuery = (q: string) => (query.value = q);

  return {
    query,
    clearQuery,
    setQuery,
  };
};
