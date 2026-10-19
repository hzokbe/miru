using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class MangaSeedData
{
    public static List<Manga> Create()
    {
        return
        [
            new Manga
            {
                Id = Guid.NewGuid(),
                Title = "Vagabond",
                JapaneseTitle = "バガボンド",
                Synopsis =
                    "Takezo, a violent and reckless youth, leaves his village in search of glory. Taking the name Miyamoto Musashi, he walks the path of the sword, and each duel and encounter pushes him to question what true strength really means.",
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
                    "Yotsuba is a curious and energetic little girl who moves to a new town with her father. Rain, cicadas, bicycles and cardboard boxes: everything becomes a fresh adventure, with the help of the friendly neighbors next door.",
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
                    "At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.",
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
                    "Two neighboring high schools, one with a rowdy reputation and the other known for its refinement, have never gotten along. A chance meeting between a boy from the first and a girl from the second begins to break down the prejudice between them.",
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
                    "After a hero's party defeats the Demon King, the long-lived elf mage Frieren outlives her old companions. Realizing she never truly got to know them, she sets out on a new journey with a young apprentice to revisit the places they once shared.",
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
                    "Punpun is an ordinary boy who grows up through a troubled family life, a first love and the pains of adolescence. As his world grows darker and more confusing, he slowly drifts toward an uncertain adulthood.",
                Chapters = 147,
                Volumes = 13,
                Status = MangaStatus.Finished,
                PublishedFrom = new DateOnly(2007, 3, 15),
                PublishedTo = new DateOnly(2013, 11, 2),
                Score = 8.97m
            }
        ];
    }
}
