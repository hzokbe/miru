using Miru.Models;

namespace Miru.Data.Seeding;

public static class MangaGenreSeedData
{
    private static readonly Dictionary<string, string[]> GenresByMangaTitle = new()
    {
        ["Vagabond"] = ["Action", "Adventure"],
        ["Yotsuba to!"] = ["Comedy", "Slice of Life"],
        ["Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen"] = ["Comedy", "Romance"],
        ["Kaoru Hana wa Rin to Saku"] = ["Drama", "Romance"],
        ["Sousou no Frieren"] = ["Adventure", "Drama", "Fantasy"],
        ["Oyasumi Punpun"] = ["Drama", "Slice of Life"]
    };

    public static List<MangaGenre> Create(List<Manga> mangas, List<Genre> genres)
    {
        var genreIdsByName = genres.ToDictionary(g => g.Name, g => g.Id);

        return
        [
            .. mangas.Where(m => GenresByMangaTitle.ContainsKey(m.Title)).SelectMany(m =>
                GenresByMangaTitle[m.Title].Select(genreName => new MangaGenre
                    { MangaId = m.Id, GenreId = genreIdsByName[genreName] }
                )
            )
        ];
    }
}
