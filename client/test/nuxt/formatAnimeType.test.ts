import { describe, expect, it } from 'vitest';
import { formatAnimeType } from '~/utils/format-anime-type';
import type { AnimeType } from '~/types/anime-response.ts';

describe('formatAnimeType', () => {
  it.each([
    ['Tv', 'TV'],
    ['Ova', 'OVA'],
    ['Movie', 'Movie'],
    ['Special', 'Special'],
    ['Ona', 'ONA'],
    ['Music', 'Music'],
    ['Cm', 'CM'],
    ['Pv', 'PV'],
    ['TvSpecial', 'TV Special'],
  ] as const)('formats %s as %s', (value, expected) => {
    expect(formatAnimeType(value)).toBe(expected);
  });

  it('returns the raw value when the type is unknown', () => {
    expect(formatAnimeType('Unknown' as AnimeType)).toBe('Unknown');
  });
});
