export const formatStatus = (status: AnimeStatus) =>
  status.replace(/([a-z])([A-Z])/g, '$1 $2');

export const formatScore = (score: number | null) =>
  score === null ? '0.0' : score.toFixed(2);

export const formatEpisodes = (episodes: number | null) => {
  const count = episodes ?? 0;

  return `${count} ${count === 1 ? 'episode' : 'episodes'}`;
};

const ANIME_TYPE_LABELS: Record<AnimeType, string> = {
  Cm: 'CM',
  Movie: 'Movie',
  Music: 'Music',
  Ona: 'ONA',
  Ova: 'OVA',
  Pv: 'PV',
  Special: 'Special',
  Tv: 'TV',
  TvSpecial: 'TV Special',
};

export const formatType = (type: AnimeType | null) =>
  type ? ANIME_TYPE_LABELS[type] : 'Unknown';
