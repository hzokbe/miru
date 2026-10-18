export const useAPIHealth = () => {
  const api = useAPI();

  const toast = useToast();

  const checkHealth = async () => {
    try {
      await api('/health');

      toast.add({
        title: 'Connected',
        description: 'The API is online and responding normally.',
        icon: 'i-lucide-wifi',
        color: 'success',
        progress: false,
      });
    } catch (e) {
      console.error(e);

      toast.add({
        title: 'Unable to reach the server',
        description:
          'Something went wrong on our end. Please try again in a few moments.',
        icon: 'i-lucide-wifi-off',
        color: 'error',
        progress: false,
      });
    }
  };

  return { checkHealth };
};
