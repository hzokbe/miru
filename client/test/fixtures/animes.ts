import type { AnimeResponse } from '~/types/anime-response.ts';
import type { AnimeCardProps } from '~/components/anime/card/index.vue';

export const animeFixtures: AnimeResponse[] = [
  {
    id: 'a3f1c8e2-5b7d-4e9a-8c34-2d6f91b0e7a5',
    title: 'Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen',
    japaneseTitle: 'かぐや様は告らせたい～天才たちの恋愛頭脳戦～',
    synopsis:
      'Kaguya Shinomiya and Miyuki Shirogane lead the student council of the elite Shuchiin Academy, and each secretly has feelings for the other. Too proud to confess first, they scheme, bluff and analyze every small gesture, turning ordinary school moments into elaborate psychological duels where the first to admit their love is the loser.',
    type: 'Tv',
    episodes: 12,
    status: 'FinishedAiring',
    airedFrom: '2019-01-12',
    airedTo: '2019-03-30',
    score: 8.4,
    imageSource: null,
  },
  {
    id: '7c2e94d1-0a8b-4f63-b5d9-e1473a82c60f',
    title: 'Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen',
    japaneseTitle: 'かぐや様は告らせたい？ ～天才たちの恋愛頭脳戦～',
    synopsis:
      "As the council's term nears its end, the rivalry between Kaguya and Shirogane becomes more complicated than ever. A heated election, a looming birthday and a string of misunderstandings test their pride, while the rest of the council is drawn deeper into the endless games of wits and half-confessed feelings.",
    type: 'Tv',
    episodes: 12,
    status: 'FinishedAiring',
    airedFrom: '2020-04-11',
    airedTo: '2020-06-27',
    score: 8.61,
    imageSource: null,
  },
  {
    id: 'e5b80a37-9d14-4c2f-a7e6-48f3d0b1952c',
    title: 'Kaguya-sama wa Kokurasetai: Ultra Romantic',
    japaneseTitle: 'かぐや様は告らせたい-ウルトラロマンティック-',
    synopsis:
      "Rivalries and mix-ups continue at the council, but the stakes keep rising as Kaguya and Shirogane are pressed by their families, their futures and their own hearts. School festivals, rumors and new challenges push everyone's feelings into the open, and the game of wits slowly gives way to something far more sincere.",
    type: 'Tv',
    episodes: 13,
    status: 'FinishedAiring',
    airedFrom: '2022-04-09',
    airedTo: '2022-06-25',
    score: 8.95,
    imageSource: null,
  },
  {
    id: '2d9f6b14-c3a7-48e5-9b0d-71a5e8c43f96',
    title: 'Sousou no Frieren',
    japaneseTitle: '葬送のフリーレン',
    synopsis:
      'Decades after helping defeat the Demon King, the long-lived elf mage Frieren watches her old companions grow old and go their separate ways. When the hero Himmel dies, she realizes how little she bothered to know him and the others during their ten-year journey. Determined to understand humans, she sets out on a new quest with her apprentice Fern and the warrior Stark, retracing old paths and discovering the meaning of the memories she left behind.',
    type: 'Tv',
    episodes: 28,
    status: 'FinishedAiring',
    airedFrom: '2023-09-29',
    airedTo: '2024-03-22',
    score: 9.24,
    imageSource: null,
  },
  {
    id: '94a7d0e8-61bc-4f2a-8d35-b0c9172e5a4d',
    title: 'Sousou no Frieren 2nd Season',
    japaneseTitle: '葬送のフリーレン 第2期',
    synopsis:
      "The party presses north toward the lands where Frieren's old comrades once ventured, facing powerful foes and increasingly dangerous trials along the way. As the journey grows harder, Frieren reflects on how much she has changed since Himmel's death, while Fern and Stark confront their own growth, doubts and the bond they share with their teacher.",
    type: 'Tv',
    episodes: 10,
    status: 'FinishedAiring',
    airedFrom: '2026-01-16',
    airedTo: '2026-03-27',
    score: 8.84,
    imageSource: null,
  },
  {
    id: '0f3c5a92-e8d6-4b71-a249-5c86b3d7e1f0',
    title: 'Steins;Gate',
    japaneseTitle: 'STEINS;GATE',
    synopsis:
      'Rintaro Okabe, a self-proclaimed mad scientist, runs a tiny lab in Akihabara with his friends. When he discovers that a modified microwave can send text messages to the past, small experiments begin to change the present in unexpected ways. Soon a shadowy organization closes in, and Okabe must face the dangerous consequences of altering time to protect the people he cares about.',
    type: 'Tv',
    episodes: 24,
    status: 'FinishedAiring',
    airedFrom: '2011-04-06',
    airedTo: '2011-09-14',
    score: 9.07,
    imageSource: null,
  },
  {
    id: 'b61e47c3-2fa9-4d08-9e5b-a3d70c8f2146',
    title: 'Steins;Gate 0',
    japaneseTitle: 'シュタインズ・ゲート ゼロ',
    synopsis:
      'Shaken by a tragic failure to save someone he loves, Okabe tries to live an ordinary life, haunted by guilt and by memories of the timelines he has seen. His fragile peace is broken when he is drawn into an AI research project that replicates human memories, a technology that may hold the key to the future and raises dangerous questions about time, loss and the choices he can never undo.',
    type: 'Tv',
    episodes: 23,
    status: 'FinishedAiring',
    airedFrom: '2018-04-12',
    airedTo: '2018-09-27',
    score: 8.56,
    imageSource: null,
  },
  {
    id: '5e8a2d70-b4c1-49f6-86a3-d92f0e17b5c8',
    title: 'Fullmetal Alchemist: Brotherhood',
    japaneseTitle: '鋼の錬金術師 FULLMETAL ALCHEMIST',
    synopsis:
      "After a forbidden attempt to revive their dead mother, the alchemist brothers Edward and Alphonse Elric pay a terrible price: Edward loses an arm and a leg, and Alphonse loses his whole body, his soul bound to a suit of armor. Seeking the legendary Philosopher's Stone to restore what they lost, they join the State Military and uncover a vast conspiracy that threatens the entire country.",
    type: 'Tv',
    episodes: 64,
    status: 'FinishedAiring',
    airedFrom: '2009-04-05',
    airedTo: '2010-07-04',
    score: 9.11,
    imageSource: null,
  },
  {
    id: 'c1d93f56-7a2e-4b84-a0f7-36e5b8d1294a',
    title: 'Chainsaw Man',
    japaneseTitle: 'チェンソーマン',
    synopsis:
      "Denji is a young man crushed by his late father's debt, scraping by as a devil hunter alongside his devil companion Pochita. After a betrayal leaves him dying, he merges with Pochita and becomes Chainsaw Man, a being with a chainsaw heart. Recruited by the government's Public Safety Devil Hunters, he gets a chance at a better life, but is pulled into a brutal world of devils, shifting loyalties and dangerous people.",
    type: 'Tv',
    episodes: 12,
    status: 'FinishedAiring',
    airedFrom: '2022-10-12',
    airedTo: '2022-12-28',
    score: 8.41,
    imageSource: null,
  },
];

export const createAnimeFixture = (
  overrides: Partial<AnimeResponse> = {},
): AnimeResponse => ({
  ...animeFixtures[3]!,
  ...overrides,
});

export const animeCardFixtures: AnimeCardProps[] = animeFixtures.map(
  ({ id, title, score, episodes, status, type, imageSource }) => ({
    id,
    title,
    score,
    episodes,
    status,
    type,
    imageSource,
  }),
);
