import { beforeEach, describe, expect, it, vi } from 'vitest';
import { mockNuxtImport } from '@nuxt/test-utils/runtime';
import { useAPIHealth } from '~/composables/useAPIHealth';

const { apiMock, toastAddMock } = vi.hoisted(() => ({
  apiMock: vi.fn(),
  toastAddMock: vi.fn(),
}));

mockNuxtImport('useAPI', () => () => apiMock);

mockNuxtImport('useToast', () => () => ({ add: toastAddMock }));

describe('useAPIHealth', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('calls the health endpoint', async () => {
    apiMock.mockResolvedValue({});

    const { checkHealth } = useAPIHealth();

    await checkHealth();

    expect(apiMock).toHaveBeenCalledTimes(1);

    expect(apiMock).toHaveBeenCalledWith('/health');
  });

  it('shows a success toast when the API responds', async () => {
    apiMock.mockResolvedValue({});

    const { checkHealth } = useAPIHealth();

    await checkHealth();

    expect(toastAddMock).toHaveBeenCalledTimes(1);

    expect(toastAddMock).toHaveBeenCalledWith({
      title: 'Connected',
      description: 'The API is online and responding normally.',
      icon: 'i-lucide-wifi',
      color: 'success',
      progress: false,
    });
  });

  it('shows an error toast when the API fails', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    apiMock.mockRejectedValue(new Error('Network error'));

    const { checkHealth } = useAPIHealth();

    await checkHealth();

    expect(toastAddMock).toHaveBeenCalledTimes(1);

    expect(toastAddMock).toHaveBeenCalledWith({
      title: 'Unable to reach the server',
      description:
        'Something went wrong on our end. Please try again in a few moments.',
      icon: 'i-lucide-wifi-off',
      color: 'error',
      progress: false,
    });
  });

  it('logs the error when the API fails', async () => {
    const error = new Error('Network error');

    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});

    apiMock.mockRejectedValue(error);

    const { checkHealth } = useAPIHealth();

    await checkHealth();

    expect(consoleSpy).toHaveBeenCalledWith(error);
  });

  it('does not show a success toast when the API fails', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    apiMock.mockRejectedValue(new Error('Network error'));

    const { checkHealth } = useAPIHealth();

    await checkHealth();

    expect(toastAddMock).not.toHaveBeenCalledWith(
      expect.objectContaining({ color: 'success' }),
    );
  });
});
