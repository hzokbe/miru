import type { GenreResponse } from '~/types/genre-response.ts';

export type AnimeType =
  | 'Tv'
  | 'Ova'
  | 'Movie'
  | 'Special'
  | 'Ona'
  | 'Music'
  | 'Cm'
  | 'Pv'
  | 'TvSpecial';

export type AnimeStatus = 'FinishedAiring' | 'CurrentlyAiring' | 'NotYetAired';

export interface AnimeResponse {
  id: string;

  title: string;

  japaneseTitle: string | null;

  synopsis: string | null;

  type: AnimeType;

  episodes: number | null;

  status: AnimeStatus;

  airedFrom: string | null;

  airedTo: string | null;

  score: number | null;

  imageSource: string | null;

  genres: GenreResponse[];
}
