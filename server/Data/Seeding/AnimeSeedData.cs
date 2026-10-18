using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class AnimeSeedData
{
    public static List<Anime> Create()
    {
        return
        [
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
                JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
                Synopsis = "Two student council geniuses wage a battle of wits to make the other confess first.",
                Type = AnimeType.Tv,
                Episodes = 12,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2019, 1, 12),
                AiredTo = new DateOnly(2019, 3, 30),
                Score = 8.40m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen",
                JapaneseTitle = "かぐや様は告らせたい？ ～天才たちの恋愛頭脳戦～",
                Synopsis = "As the council's term nears its end, a heated election and a birthday complicate the game.",
                Type = AnimeType.Tv,
                Episodes = 12,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2020, 4, 11),
                AiredTo = new DateOnly(2020, 6, 27),
                Score = 8.61m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Kaguya-sama wa Kokurasetai: Ultra Romantic",
                JapaneseTitle = "かぐや様は告らせたい-ウルトラロマンティック-",
                Synopsis = "Rivalries and mix-ups continue at the council while everyone's feelings become clearer.",
                Type = AnimeType.Tv,
                Episodes = 13,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2022, 4, 9),
                AiredTo = new DateOnly(2022, 6, 25),
                Score = 8.95m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Sousou no Frieren",
                JapaneseTitle = "葬送のフリーレン",
                Synopsis =
                    "Decades after defeating the Demon King, the elf Frieren realizes how much she overlooked her companions and sets out to understand humans.",
                Type = AnimeType.Tv,
                Episodes = 28,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2023, 9, 29),
                AiredTo = new DateOnly(2024, 3, 22),
                Score = 9.24m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Sousou no Frieren 2nd Season",
                JapaneseTitle = "葬送のフリーレン 第2期",
                Synopsis =
                    "The party presses north against powerful foes while Frieren reflects on how much she has changed.",
                Type = AnimeType.Tv,
                Episodes = 10,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2026, 1, 16),
                AiredTo = new DateOnly(2026, 3, 27),
                Score = 8.84m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Steins;Gate",
                JapaneseTitle = "STEINS;GATE",
                Synopsis =
                    "A self-proclaimed mad scientist finds that his microwave can send messages to the past, with dangerous consequences.",
                Type = AnimeType.Tv,
                Episodes = 24,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2011, 4, 6),
                AiredTo = new DateOnly(2011, 9, 14),
                Score = 9.07m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Steins;Gate 0",
                JapaneseTitle = "シュタインズ・ゲート ゼロ",
                Synopsis =
                    "Shaken by a tragic failure, Okabe tries to live an ordinary life until an AI project that replicates human memories pulls him back in.",
                Type = AnimeType.Tv,
                Episodes = 23,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2018, 4, 12),
                AiredTo = new DateOnly(2018, 9, 27),
                Score = 8.56m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Fullmetal Alchemist: Brotherhood",
                JapaneseTitle = "鋼の錬金術師 FULLMETAL ALCHEMIST",
                Synopsis =
                    "Two brothers pay a terrible price for trying to revive their mother and seek the Philosopher's Stone to restore their bodies.",
                Type = AnimeType.Tv,
                Episodes = 64,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2009, 4, 5),
                AiredTo = new DateOnly(2010, 7, 4),
                Score = 9.11m
            },
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Chainsaw Man",
                JapaneseTitle = "チェンソーマン",
                Synopsis =
                    "Indebted and out of options, Denji merges with the devil Pochita and becomes a devil hunter for the government.",
                Type = AnimeType.Tv,
                Episodes = 12,
                Status = AnimeStatus.FinishedAiring,
                AiredFrom = new DateOnly(2022, 10, 12),
                AiredTo = new DateOnly(2022, 12, 28),
                Score = 8.41m
            }
        ];
    }
}
