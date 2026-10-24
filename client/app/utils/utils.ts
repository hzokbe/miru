export const formatScore = (score: number | null) =>
  score === null ? '0.0' : score.toFixed(2);

export const formatStatus = (status: AnimeStatus | MangaStatus) =>
  status.replace(/([a-z])([A-Z])/g, '$1 $2');

export const formatDate = (value: string | null): string => {
  if (!value) {
    return '?';
  }

  const date = new Date(value);

  if (Number.isNaN(date.getTime())) {
    return '?';
  }

  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: '2-digit',
    timeZone: 'UTC',
  });
};
