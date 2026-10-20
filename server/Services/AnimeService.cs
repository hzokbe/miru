using Miru.DTO;
using Miru.Models;
using Miru.Repositories;

namespace Miru.Services;

public class AnimeService(IAnimeRepository repository) : IAnimeService
{
    public async Task<List<AnimeResponse>> GetAllAsync()
    {
        return [.. (await repository.GetAllAsync()).Select(ToResponse)];
    }

    public async Task<AnimeResponse?> GetByIdAsync(Guid id)
    {
        var anime = await repository.GetByIdAsync(id);

        return anime != null ? ToResponse(anime) : null;
    }

    private static AnimeResponse ToResponse(Anime anime)
    {
        return new AnimeResponse(
            anime.Id,
            anime.Title,
            anime.JapaneseTitle,
            anime.Synopsis,
            anime.Type,
            anime.Episodes,
            anime.Status,
            anime.AiredFrom,
            anime.AiredTo,
            anime.Score,
            anime.ImageSource,
            [.. anime.AnimeGenres.Select(ag => ToResponse(ag.Genre))]
        );
    }

    private static GenreResponse ToResponse(Genre genre)
    {
        return new GenreResponse(
            genre.Id,
            genre.Name,
            genre.Description
        );
    }
}
