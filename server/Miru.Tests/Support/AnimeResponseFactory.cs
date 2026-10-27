using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public static class AnimeResponseFactory
{
    public static AnimeResponse From(Anime anime)
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
            [.. anime.AnimeCharacters.Select(ac => CharacterResponseFactory.From(ac.Character))]
        );
    }
}
