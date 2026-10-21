using Miru.Models;

namespace Miru.Data.Seeding;

public static class MangaCharacterSeedData
{
    private static readonly Dictionary<string, string[]> CharactersByMangaTitle = new()
    {
        ["Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen"] = ["Kaguya", "Miyuki"],
        ["Sousou no Frieren"] = ["Frieren", "Fern", "Stark"]
    };

    public static List<MangaCharacter> Create(List<Manga> mangas, List<Character> characters)
    {
        var characterIdsByName = characters.ToDictionary(c => c.Name, c => c.Id);

        return
        [
            .. mangas.Where(a => CharactersByMangaTitle.ContainsKey(a.Title)).SelectMany(a =>
                CharactersByMangaTitle[a.Title].Select(characterName => new MangaCharacter
                    { MangaId = a.Id, CharacterId = characterIdsByName[characterName] }
                )
            )
        ];
    }
}
