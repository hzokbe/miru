using Miru.DTO;
using Miru.Models;
using Miru.Repositories;

namespace Miru.Services;

public class SearchService(
    IAnimeRepository animeRepository,
    IMangaRepository mangaRepository,
    ICharacterRepository characterRepository
) : ISearchService
{
    public async Task<SearchResponse> SearchAsync(string query)
    {
        var animes = await animeRepository.SearchAsync(query);

        var mangas = await mangaRepository.SearchAsync(query);

        var characters = await characterRepository.SearchAsync(query);

        return new SearchResponse(
            [.. animes.Select(ToResponse)],
            [.. mangas.Select(ToResponse)],
            [.. characters.Select(ToResponse)]
        );
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
            [.. anime.AnimeCharacters.Select(ac => ToResponse(ac.Character))]
        );
    }

    private static MangaResponse ToResponse(Manga manga)
    {
        return new MangaResponse(
            manga.Id,
            manga.Title,
            manga.JapaneseTitle,
            manga.Synopsis,
            manga.Chapters,
            manga.Volumes,
            manga.Status,
            manga.PublishedFrom,
            manga.PublishedTo,
            manga.Score,
            manga.ImageSource,
            [.. manga.MangaCharacters.Select(mc => ToResponse(mc.Character))]
        );
    }

    private static CharacterResponse ToResponse(Character character)
    {
        return new CharacterResponse(
            character.Id,
            character.Name,
            character.LastName,
            character.JapaneseName,
            character.JapaneseLastName,
            character.Age,
            character.Gender,
            character.Role,
            character.Description,
            character.ImageSource
        );
    }
}
