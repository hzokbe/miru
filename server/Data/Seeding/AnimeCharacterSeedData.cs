using Miru.Models;

namespace Miru.Data.Seeding;

public static class AnimeCharacterSeedData
{
    private static readonly Dictionary<string, string[]> CharactersByAnimeTitle = new()
    {
        ["Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen"] = ["Kaguya", "Miyuki"],
        ["Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen"] = ["Kaguya", "Miyuki"],
        ["Kaguya-sama wa Kokurasetai: Ultra Romantic"] = ["Kaguya", "Miyuki"],
        ["Sousou no Frieren"] = ["Frieren", "Fern", "Stark"],
        ["Sousou no Frieren 2nd Season"] = ["Frieren", "Fern", "Stark"],
        ["Steins;Gate"] = ["Rintarou", "Kurisu", "Mayuri"],
        ["Steins;Gate 0"] = ["Rintarou", "Kurisu", "Mayuri"],
        ["Fullmetal Alchemist: Brotherhood"] = ["Edward", "Alphonse"],
        ["Chainsaw Man"] = ["Denji", "Power", "Makima"]
    };

    public static List<AnimeCharacter> Create(List<Anime> animes, List<Character> characters)
    {
        var characterIdsByName = characters.ToDictionary(c => c.Name, c => c.Id);

        return
        [
            .. animes.Where(a => CharactersByAnimeTitle.ContainsKey(a.Title)).SelectMany(a =>
                CharactersByAnimeTitle[a.Title].Select(characterName => new AnimeCharacter
                    { AnimeId = a.Id, CharacterId = characterIdsByName[characterName] }
                )
            )
        ];
    }
}
