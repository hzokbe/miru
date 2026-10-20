using Miru.Models;

namespace Miru.Data.Seeding;

public static class AnimeGenreSeedData
{
    private static readonly Dictionary<string, string[]> GenresByAnimeTitle = new()
    {
        ["Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen"] = ["Comedy", "Romance"],
        ["Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen"] = ["Comedy", "Romance"],
        ["Kaguya-sama wa Kokurasetai: Ultra Romantic"] = ["Comedy", "Romance"],
        ["Sousou no Frieren"] = ["Adventure", "Drama", "Fantasy"],
        ["Sousou no Frieren 2nd Season"] = ["Adventure", "Drama", "Fantasy"],
        ["Steins;Gate"] = ["Drama", "Sci-Fi", "Suspense"],
        ["Steins;Gate 0"] = ["Drama", "Sci-Fi", "Suspense"],
        ["Fullmetal Alchemist: Brotherhood"] = ["Action", "Adventure", "Drama", "Fantasy"],
        ["Chainsaw Man"] = ["Action", "Fantasy"]
    };

    public static List<AnimeGenre> Create(List<Anime> animes, List<Genre> genres)
    {
        var genreIdsByName = genres.ToDictionary(g => g.Name, g => g.Id);

        return
        [
            .. animes.Where(a => GenresByAnimeTitle.ContainsKey(a.Title)).SelectMany(a =>
                GenresByAnimeTitle[a.Title].Select(genreName => new AnimeGenre
                    { AnimeId = a.Id, GenreId = genreIdsByName[genreName] }
                )
            )
        ];
    }
}
