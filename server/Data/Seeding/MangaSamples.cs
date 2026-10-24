using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class MangaSamples
{
    public static List<Manga> CreateAll()
    {
        return
        [
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Vagabond",
                JapaneseTitle = "バガボンド",
                Synopsis =
                    "Takezo, a violent and reckless youth, leaves his village in search of glory after the great battle of Sekigahara. Hunted as an outlaw, he is eventually captured and reshaped by a wise monk, who gives him a new beginning under the name Miyamoto Musashi. From then on he walks the path of the sword, travelling across feudal Japan and facing famous schools and fearsome duelists. Each duel and each encounter, whether with allies, rivals or ordinary people, pushes him to question what true strength really means and what it costs to become invincible.",
                Chapters = 327,
                Volumes = 37,
                Status = MangaStatus.OnHiatus,
                PublishedFrom = new DateOnly(1998, 9, 3),
                PublishedTo = new DateOnly(2015, 5, 21),
                Score = 9.27m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Yotsuba to!",
                JapaneseTitle = "よつばと!",
                Synopsis =
                    "Yotsuba is a curious and energetic little girl who moves to a new town with her easygoing father. Everything is new and astonishing to her: rain, cicadas, swings, bicycles, air conditioners and even cardboard boxes become fresh adventures. With the help of the friendly neighbors next door, a tall family friend and a cast of warm, quirky characters, she discovers the small wonders of everyday life, turning ordinary days into unforgettable and hilarious episodes full of innocence and joy.",
                Chapters = null,
                Volumes = null,
                Status = MangaStatus.Publishing,
                PublishedFrom = new DateOnly(2003, 1, 21),
                PublishedTo = null,
                Score = 8.92m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
                JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
                Synopsis =
                    "At an elite academy for the gifted, the two top students of the student council, the poised vice president from a wealthy family and the hardworking president, are secretly attracted to each other. Too proud to confess first, since doing so would mean admitting defeat, both scheme and bluff in elaborate mind games, each hoping the other will give in. As the school year goes on, their absurd tactics, misunderstandings and the colorful people around them keep turning simple everyday situations into full-scale psychological warfare, while their real feelings grow harder to hide.",
                Chapters = 281,
                Volumes = 28,
                Status = MangaStatus.Finished,
                PublishedFrom = new DateOnly(2015, 5, 19),
                PublishedTo = new DateOnly(2022, 11, 2),
                Score = 8.88m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Kaoru Hana wa Rin to Saku",
                JapaneseTitle = "薫る花は凛と咲く",
                Synopsis =
                    "Two neighboring high schools, one with a rowdy reputation and the other known for its refinement and high standards, have never gotten along. Students from each side keep their distance, held back by rumors and long-standing prejudice. A chance meeting between a gentle, intimidating-looking boy from the first school and a kind, open-minded girl from the second begins to break down those walls. Through small acts of kindness, shared moments and growing trust, they slowly show each other, and the people around them, that first impressions rarely tell the whole story.",
                Chapters = null,
                Volumes = null,
                Status = MangaStatus.Publishing,
                PublishedFrom = new DateOnly(2021, 10, 21),
                PublishedTo = null,
                Score = 8.76m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Sousou no Frieren",
                JapaneseTitle = "葬送のフリーレン",
                Synopsis =
                    "After a hero's party defeats the Demon King and returns home in triumph, the long-lived elf mage Frieren watches the decades pass and outlives her old companions. When the hero finally dies of old age, she realizes she never truly got to know the people she travelled with, having treated ten years of adventure as a brief moment. Determined to understand them and the meaning of human lives, she sets out on a new journey with a young apprentice and new companions, revisiting the places they once shared and finding the memories they left behind.",
                Chapters = 147,
                Volumes = 15,
                Status = MangaStatus.OnHiatus,
                PublishedFrom = new DateOnly(2020, 4, 28),
                PublishedTo = new DateOnly(2025, 10, 15),
                Score = 8.87m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Oyasumi Punpun",
                JapaneseTitle = "おやすみプンプン",
                Synopsis =
                    "Punpun is an ordinary boy, drawn as a simple bird-like doodle, who grows up through a troubled family life, a tender first love and the many pains of adolescence. Guided only by an imaginary figure he calls God and by the people who drift in and out of his life, he tries to make sense of loneliness, shame and desire. As his world grows darker and more confusing, and as his choices and circumstances weigh on him, he slowly drifts toward an uncertain adulthood, in a story that mixes dark humor, quiet moments and deep emotional honesty.",
                Chapters = 147,
                Volumes = 13,
                Status = MangaStatus.Finished,
                PublishedFrom = new DateOnly(2007, 3, 15),
                PublishedTo = new DateOnly(2013, 11, 2),
                Score = 8.97m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Shoujo Shuumatsu Ryokou",
                JapaneseTitle = "少女終末旅行",
                Synopsis =
                    "Chito and Yuuri are two girls wandering the ruins of a civilization that has already vanished. Riding a battered half-track motorcycle, they search empty cities for food, fuel and shelter, surviving day by day in a world of snow, concrete and silence. Cheerful Yuuri and level-headed Chito find small joys in meals, machines and forgotten relics along the way, while a quiet question follows them everywhere: whether anyone else is still out there, and what, if anything, remains of humanity's story.",
                Chapters = 47,
                Volumes = 6,
                Status = MangaStatus.Finished,
                PublishedFrom = new DateOnly(2014, 2, 21),
                PublishedTo = new DateOnly(2018, 1, 12),
                Score = 8.75m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Chainsaw Man",
                JapaneseTitle = "チェンソーマン",
                Synopsis =
                    "Denji is a poor, desperate teenager who works off his late father's debt to the yakuza by hunting devils alongside Pochita, a small chainsaw-tailed companion. After a betrayal leaves him dead, Pochita gives him its heart, and Denji returns as a being who can turn his body into chainsaws. Recruited by the government's devil hunters, he dreams of nothing more than warm bread, a roof over his head and a girlfriend, but the violent, bizarre world he enters keeps testing how far such simple wishes can carry him.",
                Chapters = 232,
                Volumes = 24,
                Status = MangaStatus.Finished,
                PublishedFrom = new DateOnly(2018, 12, 3),
                PublishedTo = new DateOnly(2026, 3, 25),
                Score = 8.61m
            },
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Shimeji Simulation",
                JapaneseTitle = "シメジ シミュレーション",
                Synopsis =
                    "After shutting herself away for two years, Shijima finally steps back into the world to start high school, only to discover that mushrooms have sprouted on her head. Anxious and awkward around others, she plans to keep her head down and avoid attention, until the eccentric Majime sticks to her side and refuses to be ignored. Between odd school days, strange discoveries and an unlikely friendship, Shijima slowly learns that being seen, mushrooms and all, may not be as frightening as she believed.",
                Chapters = 49,
                Volumes = 5,
                Status = MangaStatus.Finished,
                PublishedFrom = new DateOnly(2019, 1, 26),
                PublishedTo = new DateOnly(2023, 11, 27),
                Score = 8.55m
            }
        ];
    }
}
