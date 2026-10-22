export const useAPI = () => {
  const config = useRuntimeConfig();

  return $fetch.create({
    baseURL: config.public.apiURL,
    headers: {
      Accept: 'application/json',
    },
    retry: 0,
  });
};
