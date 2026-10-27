export const useCharacter = (id: string) => {
  const api = useAPI();

  const {
    data: character,
    error,
    status,
    refresh,
  } = useAsyncData(
    `character-${id}`,
    async () => {
      try {
        return await api<CharacterResponse>(`/characters/${id}`);
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
    character,
    error,
    status,
    refresh,
    loading,
  };
};
