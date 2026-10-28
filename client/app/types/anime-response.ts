export type AnimeType =
  | 'Cm'
  | 'Movie'
  | 'Music'
  | 'Ona'
  | 'Ova'
  | 'Pv'
  | 'Special'
  | 'Tv'
  | 'TvSpecial';

export type AnimeStatus = 'CurrentlyAiring' | 'FinishedAiring' | 'NotYetAired';

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

  characters: CharacterResponse[];

  genres: GenreResponse[];
}
