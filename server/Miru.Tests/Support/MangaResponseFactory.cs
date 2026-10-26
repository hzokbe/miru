using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public static class MangaResponseFactory
{
    public static MangaResponse From(Manga manga)
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
