using Miru.Models;

namespace Miru.Data.Seeding;

public static class AnimeGenreSamples
{
    private const string KaguyaSamaFirstSeason = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen";

    private const string KaguyaSamaSecondSeason = "Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen";

    private const string KaguyaSamaThirdSeason = "Kaguya-sama wa Kokurasetai: Ultra Romantic";

    private const string SousouNoFrierenFirstSeason = "Sousou no Frieren";

    private const string SteinsGate = "Steins;Gate";

    private const string SteinsGateZero = "Steins;Gate 0";

    private const string ChainsawMan = "Chainsaw Man";

    public static List<AnimeGenre> CreateAll(List<Anime> animes, List<Genre> genres)
    {
        var animeByTitle = animes.ToDictionary(anime => anime.Title);

        var genreByName = genres.ToDictionary(genre => genre.Name);

        var links = new (string Anime, string Genre)[]
        {
            (KaguyaSamaFirstSeason, "Comedy"),
            (KaguyaSamaFirstSeason, "Romance"),

            (KaguyaSamaSecondSeason, "Comedy"),
            (KaguyaSamaSecondSeason, "Romance"),

            (KaguyaSamaThirdSeason, "Comedy"),
            (KaguyaSamaThirdSeason, "Romance"),

            (SousouNoFrierenFirstSeason, "Adventure"),
            (SousouNoFrierenFirstSeason, "Award Winning"),
            (SousouNoFrierenFirstSeason, "Drama"),
            (SousouNoFrierenFirstSeason, "Fantasy"),

            (SteinsGate, "Drama"),
            (SteinsGate, "Sci-Fi"),
            (SteinsGate, "Suspense"),

            (SteinsGateZero, "Drama"),
            (SteinsGateZero, "Sci-Fi"),
            (SteinsGateZero, "Suspense"),

            (ChainsawMan, "Action"),
            (ChainsawMan, "Adventure"),
            (ChainsawMan, "Award Winning")
        };

        return
        [
            .. links.Select(ag => ToAnimeGenre(animeByTitle[ag.Anime].Id, genreByName[ag.Genre].Id))
        ];
    }

    private static AnimeGenre ToAnimeGenre(Guid animeId, Guid genreId)
    {
        return new AnimeGenre
        {
            AnimeId = animeId,
            GenreId = genreId
        };
    }
}
