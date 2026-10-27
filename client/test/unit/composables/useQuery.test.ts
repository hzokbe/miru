import { beforeEach, describe, expect, it } from 'vitest';

describe('useQuery', () => {
  beforeEach(() => {
    clearNuxtState('query');
  });

  it('starts with an empty query', () => {
    const { query } = useQuery();

    expect(query.value).toBe('');
  });

  it('sets the query', () => {
    const { query, setQuery } = useQuery();

    setQuery('naruto');

    expect(query.value).toBe('naruto');
  });

  it('clears the query', () => {
    const { query, setQuery, clearQuery } = useQuery();

    setQuery('naruto');

    clearQuery();

    expect(query.value).toBe('');
  });

  it('shares the state between calls', () => {
    const first = useQuery();

    const second = useQuery();

    first.setQuery('bleach');

    expect(second.query.value).toBe('bleach');
  });
});
