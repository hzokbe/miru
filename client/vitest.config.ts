import { defineConfig } from 'vitest/config';
import { defineVitestProject } from '@nuxt/test-utils/config';

export default defineConfig({
  test: {
    projects: [
      await defineVitestProject({
        test: {
          name: { label: 'unit', color: 'green' },
          include: ['test/unit/**/*.test.ts'],
          environment: 'nuxt',
        },
      }),
      await defineVitestProject({
        test: {
          name: { label: 'integration', color: 'blue' },
          include: ['test/integration/**/*.test.ts'],
          environment: 'nuxt',
        },
      }),
      {
        test: {
          name: { label: 'e2e', color: 'magenta' },
          include: ['test/e2e/**/*.test.ts'],
          environment: 'node',
        },
      },
    ],
    maxWorkers: 1,
  },
});
