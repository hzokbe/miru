import { defineEventHandler, proxyRequest } from 'h3';
import { useRuntimeConfig } from 'nitropack/runtime';

export default defineEventHandler((event) => {
  const { apiInternalURL } = useRuntimeConfig(event);

  const path = event.path.replace(/^\/api/, '');

  return proxyRequest(event, `${apiInternalURL}${path}`);
});
