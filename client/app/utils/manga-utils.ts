export const formatVolumes = (volumes: number | null) => {
  const count = volumes ?? 0;

  return `${count} ${count === 1 ? 'volume' : 'volumes'}`;
};

export const formatChapters = (chapters: number | null) => {
  const count = chapters ?? 0;

  return `${count} ${count === 1 ? 'chapter' : 'chapters'}`;
};
