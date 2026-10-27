import { mockNuxtImport, mountSuspended } from '@nuxt/test-utils/runtime';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defineComponent, h } from 'vue';
import { characterFixtures } from '~~/test/fixtures/characters.ts';

const { apiMock } = vi.hoisted(() => ({ apiMock: vi.fn() }));

mockNuxtImport('useAPI', () => () => apiMock);

const character = characterFixtures[0]!;

const mountComposable = async (id = character.id) => {
  let result!: ReturnType<typeof useCharacter>;

  let initialLoading = false;

  await mountSuspended(
    defineComponent({
      setup() {
        result = useCharacter(id);

        initialLoading = result.loading.value;

        return () => h('div');
      },
    }),
  );

  return { ...result, initialLoading };
};

describe('useCharacter', () => {
  beforeEach(() => {
    apiMock.mockReset();

    clearNuxtData();
  });

  it('requests the character endpoint with the id', async () => {
    apiMock.mockResolvedValue(character);

    const { status } = await mountComposable('7');

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledWith('/characters/7');
  });

  it('exposes the fetched character', async () => {
    apiMock.mockResolvedValue(character);

    const { character: data, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value).toEqual(character);
  });

  it('defaults to null while loading', async () => {
    apiMock.mockReturnValue(new Promise(() => {}));

    const { character: data } = await mountComposable();

    expect(data.value).toBeNull();
  });

  it('is loading while pending and stops after success', async () => {
    let resolve!: (value: typeof character) => void;

    apiMock.mockReturnValue(
      new Promise<typeof character>((r) => {
        resolve = r;
      }),
    );

    const { initialLoading, loading } = await mountComposable();

    expect(initialLoading).toBe(true);

    resolve(character);

    await vi.waitFor(() => expect(loading.value).toBe(false));
  });

  it('returns null without error when the API responds 404', async () => {
    apiMock.mockRejectedValue({ statusCode: 404 });

    const { character: data, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value).toBeNull();

    expect(error.value).toBeUndefined();

    expect(loading.value).toBe(false);
  });

  it('exposes the error and keeps null on other failures', async () => {
    apiMock.mockRejectedValue(new Error('request failed'));

    const { character: data, error, loading, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(data.value).toBeNull();

    expect(loading.value).toBe(false);
  });

  it('rethrows non-404 status codes as errors', async () => {
    apiMock.mockRejectedValue({ statusCode: 500 });

    const { character: data, error, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('error'));

    expect(error.value).toBeTruthy();

    expect(data.value).toBeNull();
  });

  it('uses a distinct cache key per id', async () => {
    apiMock.mockResolvedValue(character);

    const first = await mountComposable('1');

    await vi.waitFor(() => expect(first.status.value).toBe('success'));

    const second = await mountComposable('2');

    await vi.waitFor(() => expect(second.status.value).toBe('success'));

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(apiMock).toHaveBeenCalledWith('/characters/1');

    expect(apiMock).toHaveBeenCalledWith('/characters/2');
  });

  it('refetches on refresh', async () => {
    apiMock.mockResolvedValueOnce({ ...character, name: 'first' });

    apiMock.mockResolvedValueOnce(character);

    const { character: data, refresh, status } = await mountComposable();

    await vi.waitFor(() => expect(status.value).toBe('success'));

    expect(data.value?.name).toBe('first');

    await refresh();

    expect(apiMock).toHaveBeenCalledTimes(2);

    expect(data.value).toEqual(character);
  });
});
