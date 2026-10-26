using Miru.Models;

namespace Miru.Tests.Support;

public static class MangaCharacterLinker
{
    public static Manga WithCharacters(Manga manga, IEnumerable<Character> characters)
    {
        manga.MangaCharacters = [.. characters.Select(manga.ToMangaCharacter)];

        return manga;
    }

    private static MangaCharacter ToMangaCharacter(this Manga manga, Character character)
    {
        return new MangaCharacter
        {
            MangaId = manga.Id,
            Manga = manga,
            CharacterId = character.Id,
            Character = character
        };
    }
}
