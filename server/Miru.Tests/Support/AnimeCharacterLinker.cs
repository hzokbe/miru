using Miru.Models;

namespace Miru.Tests.Support;

public static class AnimeCharacterLinker
{
    public static Anime WithCharacters(Anime anime, IEnumerable<Character> characters)
    {
        anime.AnimeCharacters = [.. characters.Select(anime.ToAnimeCharacter)];

        return anime;
    }

    private static AnimeCharacter ToAnimeCharacter(this Anime anime, Character character)
    {
        return new AnimeCharacter
        {
            AnimeId = anime.Id,
            Anime = anime,
            CharacterId = character.Id,
            Character = character
        };
    }
}
