export type MangaStatus =
  'Publishing' | 'OnHiatus' | 'Finished' | 'Discontinued' | 'NotYetPublished';

export interface MangaResponse {
  id: string;

  title: string;

  japaneseTitle: string | null;

  synopsis: string | null;

  chapters: number | null;

  volumes: number | null;

  status: MangaStatus;

  publishedFrom: string | null;

  publishedTo: string | null;

  score: number | null;

  imageSource: string | null;

  genres: GenreResponse[];
}
