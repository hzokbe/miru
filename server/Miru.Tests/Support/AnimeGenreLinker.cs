using Miru.Models;

namespace Miru.Tests.Support;

public static class AnimeGenreLinker
{
    public static Anime WithGenres(Anime anime, IEnumerable<Genre> genres)
    {
        anime.AnimeGenres =
        [
            .. genres.Select(genre => new AnimeGenre
            {
                AnimeId = anime.Id,
                GenreId = genre.Id,
                Genre = genre
            })
        ];

        return anime;
    }
}
