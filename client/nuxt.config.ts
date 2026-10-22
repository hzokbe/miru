export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: [
    '@nuxt/ui',
    '@nuxt/eslint',
    '@nuxt/test-utils/module',
    'motion-v/nuxt',
  ],
  css: ['~/assets/css/main.css'],
  ssr: true,
  typescript: {
    typeCheck: true,
  },
  runtimeConfig: {
    apiInternalURL: '',
    public: {
      apiURL: '/api',
      appURL: 'http://localhost:3000',
    },
  },
});
