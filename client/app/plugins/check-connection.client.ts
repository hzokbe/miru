export default defineNuxtPlugin((nuxtApp) => {
  const { checkHealth } = useAPIHealth();

  nuxtApp.hook('app:mounted', () => {
    checkHealth();
  });
});
