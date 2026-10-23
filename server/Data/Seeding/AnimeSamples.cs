using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class AnimeSamples
{
    public static List<Anime> CreateAll()
    {
        return
        [
            new Anime
            {
                Id = Guid.NewGuid(),
                Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
                JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
                Synopsis =
                    "Kaguya Shinomiya and Miyuki Shirogane lead the student council of the elite Shuchiin Academy, and each secretly has feelings for the other. Too proud to confess first, they scheme, bluff and analyze every small gesture, turning ordinary school moments into elaborate psychological duels where the first to admit their love is the loser.",
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
                Synopsis =
                    "As the council's term nears its end, the rivalry between Kaguya and Shirogane becomes more complicated than ever. A heated election, a looming birthday and a string of misunderstandings test their pride, while the rest of the council is drawn deeper into the endless games of wits and half-confessed feelings.",
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
                Synopsis =
                    "Rivalries and mix-ups continue at the council, but the stakes keep rising as Kaguya and Shirogane are pressed by their families, their futures and their own hearts. School festivals, rumors and new challenges push everyone's feelings into the open, and the game of wits slowly gives way to something far more sincere.",
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
                    "Decades after helping defeat the Demon King, the long-lived elf mage Frieren watches her old companions grow old and go their separate ways. When the hero Himmel dies, she realizes how little she bothered to know him and the others during their ten-year journey. Determined to understand humans, she sets out on a new quest with her apprentice Fern and the warrior Stark, retracing old paths and discovering the meaning of the memories she left behind.",
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
                Title = "Steins;Gate",
                JapaneseTitle = "STEINS;GATE",
                Synopsis =
                    "Rintaro Okabe, a self-proclaimed mad scientist, runs a tiny lab in Akihabara with his friends. When he discovers that a modified microwave can send text messages to the past, small experiments begin to change the present in unexpected ways. Soon a shadowy organization closes in, and Okabe must face the dangerous consequences of altering time to protect the people he cares about.",
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
                    "Shaken by a tragic failure to save someone he loves, Okabe tries to live an ordinary life, haunted by guilt and by memories of the timelines he has seen. His fragile peace is broken when he is drawn into an AI research project that replicates human memories, a technology that may hold the key to the future and raises dangerous questions about time, loss and the choices he can never undo.",
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
                Title = "Chainsaw Man",
                JapaneseTitle = "チェンソーマン",
                Synopsis =
                    "Denji is a young man crushed by his late father's debt, scraping by as a devil hunter alongside his devil companion Pochita. After a betrayal leaves him dying, he merges with Pochita and becomes Chainsaw Man, a being with a chainsaw heart. Recruited by the government's Public Safety Devil Hunters, he gets a chance at a better life, but is pulled into a brutal world of devils, shifting loyalties and dangerous people.",
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
