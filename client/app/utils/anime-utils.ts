export const formatEpisodes = (episodes: number | null) => {
  const count = episodes ?? 0;

  return `${count} ${count === 1 ? 'episode' : 'episodes'}`;
};

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

export const formatType = (value: AnimeType): string =>
  animeTypeLabels[value] ?? value;
