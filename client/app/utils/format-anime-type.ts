import type { AnimeType } from '~/types/anime-response.ts';

const animeTypeLabels: Record<AnimeType, string> = {
  Tv: 'TV',
  Ova: 'OVA',
  Movie: 'Movie',
  Special: 'Special',
  Ona: 'ONA',
  Music: 'Music',
  Cm: 'CM',
  Pv: 'PV',
  TvSpecial: 'TV Special',
};

export const formatAnimeType = (value: AnimeType): string =>
  animeTypeLabels[value] ?? value;
