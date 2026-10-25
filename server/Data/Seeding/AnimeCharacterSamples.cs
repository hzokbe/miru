using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class AnimeCharacterSamples
{
    private const string KaguyaSamaFirstSeason = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen";

    private const string KaguyaSamaSecondSeason = "Kaguya-sama wa Kokurasetai?: Tensai-tachi no Renai Zunousen";

    private const string KaguyaSamaThirdSeason = "Kaguya-sama wa Kokurasetai: Ultra Romantic";

    private const string SousouNoFrierenFirstSeason = "Sousou no Frieren";

    private const string SteinsGate = "Steins;Gate";

    private const string SteinsGateZero = "Steins;Gate 0";

    private const string ChainsawMan = "Chainsaw Man";

    public static List<AnimeCharacter> CreateAll(List<Anime> animes, List<Character> characters)
    {
        var animeByTitle = animes.ToDictionary(anime => anime.Title);

        var characterByFullName = characters.ToDictionary(ToFullName);

        var links = new (string Anime, string Character, CharacterRole Role)[]
        {
            (KaguyaSamaFirstSeason, "Kaguya Shinomiya", CharacterRole.Main),
            (KaguyaSamaFirstSeason, "Miyuki Shirogane", CharacterRole.Main),
            (KaguyaSamaFirstSeason, "Ai Hayasaka", CharacterRole.Supporting),
            (KaguyaSamaFirstSeason, "Kei Shirogane", CharacterRole.Supporting),
            (KaguyaSamaFirstSeason, "Chika Fujiwara", CharacterRole.Main),
            (KaguyaSamaFirstSeason, "Yuu Ishigami", CharacterRole.Supporting),
            (KaguyaSamaFirstSeason, "Nagisa Kashiwagi", CharacterRole.Supporting),

            (KaguyaSamaSecondSeason, "Kaguya Shinomiya", CharacterRole.Main),
            (KaguyaSamaSecondSeason, "Miyuki Shirogane", CharacterRole.Main),
            (KaguyaSamaSecondSeason, "Miko Iino", CharacterRole.Main),
            (KaguyaSamaSecondSeason, "Ai Hayasaka", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Kei Shirogane", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Chika Fujiwara", CharacterRole.Main),
            (KaguyaSamaSecondSeason, "Yuu Ishigami", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Father Shirogane", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Nagisa Kashiwagi", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Tsubasa Tanuma", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Tsubame Koyasu", CharacterRole.Supporting),
            (KaguyaSamaSecondSeason, "Kobachi Osaragi", CharacterRole.Supporting),

            (KaguyaSamaThirdSeason, "Kaguya Shinomiya", CharacterRole.Main),
            (KaguyaSamaThirdSeason, "Miyuki Shirogane", CharacterRole.Main),
            (KaguyaSamaThirdSeason, "Miko Iino", CharacterRole.Main),
            (KaguyaSamaThirdSeason, "Ai Hayasaka", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Kei Shirogane", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Chika Fujiwara", CharacterRole.Main),
            (KaguyaSamaThirdSeason, "Yuu Ishigami", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Father Shirogane", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Maki Shijou", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Moeha Fujiwara", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Nao Hayasaka", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Nagisa Kashiwagi", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Tsubasa Tanuma", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Tsubame Koyasu", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Kobachi Osaragi", CharacterRole.Supporting),
            (KaguyaSamaThirdSeason, "Adolphe Pescarolo", CharacterRole.Supporting),

            (SousouNoFrierenFirstSeason, "Frieren", CharacterRole.Main),
            (SousouNoFrierenFirstSeason, "Fern", CharacterRole.Main),
            (SousouNoFrierenFirstSeason, "Stark", CharacterRole.Main),
            (SousouNoFrierenFirstSeason, "Himmel", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Eisen", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Heiter", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Übel", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Flamme", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Sein", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Denken", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Aura", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Draht", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Edel", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Genau", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Kanne", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Kraft", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Laufen", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Lawine", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Lügner", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Scharf", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Qual", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Serie", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Sense", CharacterRole.Supporting),
            (SousouNoFrierenFirstSeason, "Wirt", CharacterRole.Supporting),

            (SteinsGate, "Rintarou Okabe", CharacterRole.Main),
            (SteinsGate, "Kurisu Makise", CharacterRole.Main),
            (SteinsGate, "Mayuri Shiina", CharacterRole.Main),
            (SteinsGate, "Suzuha Amane", CharacterRole.Supporting),
            (SteinsGate, "Itaru Hashida", CharacterRole.Main),
            (SteinsGate, "Ruka Urushibara", CharacterRole.Supporting),
            (SteinsGate, "Rumiho Akiha", CharacterRole.Supporting),
            (SteinsGate, "Moeka Kiryuu", CharacterRole.Supporting),
            (SteinsGate, "Yuugo Tennouji", CharacterRole.Supporting),
            (SteinsGate, "Nae Tennouji", CharacterRole.Supporting),
            (SteinsGate, "Nakabachi", CharacterRole.Supporting),

            (SteinsGateZero, "Rintarou Okabe", CharacterRole.Main),
            (SteinsGateZero, "Kurisu Makise", CharacterRole.Main),
            (SteinsGateZero, "Mayuri Shiina", CharacterRole.Main),
            (SteinsGateZero, "Suzuha Amane", CharacterRole.Main),
            (SteinsGateZero, "Itaru Hashida", CharacterRole.Main),
            (SteinsGateZero, "Ruka Urushibara", CharacterRole.Supporting),
            (SteinsGateZero, "Rumiho Akiha", CharacterRole.Supporting),
            (SteinsGateZero, "Moeka Kiryuu", CharacterRole.Supporting),
            (SteinsGateZero, "Yuugo Tennouji", CharacterRole.Supporting),
            (SteinsGateZero, "Nae Tennouji", CharacterRole.Supporting),
            (SteinsGateZero, "Maho Hiyajou", CharacterRole.Supporting),
            (SteinsGateZero, "Kagari Shiina", CharacterRole.Supporting),
            (SteinsGateZero, "Yuki Amane", CharacterRole.Supporting),
            (SteinsGateZero, "Alexis Leskinen", CharacterRole.Supporting),

            (ChainsawMan, "Denji", CharacterRole.Main),
            (ChainsawMan, "Power", CharacterRole.Main),
            (ChainsawMan, "Makima", CharacterRole.Main),
            (ChainsawMan, "Aki Hayakawa", CharacterRole.Main),
            (ChainsawMan, "Reze", CharacterRole.Supporting),
            (ChainsawMan, "Kobeni Higashiyama", CharacterRole.Supporting),
            (ChainsawMan, "Pochita", CharacterRole.Supporting),
            (ChainsawMan, "Himeno", CharacterRole.Supporting),
            (ChainsawMan, "Kishibe", CharacterRole.Supporting),
            (ChainsawMan, "Akane Sawatari", CharacterRole.Supporting),
            (ChainsawMan, "Samurai Sword", CharacterRole.Supporting)
        };

        return
        [
            .. links.Select(ac => ToAnimeCharacter(animeByTitle[ac.Anime].Id, characterByFullName[ac.Character].Id))
        ];
    }

    private static string ToFullName(Character character)
    {
        return string.IsNullOrEmpty(character.LastName) ? character.Name : $"{character.Name} {character.LastName}";
    }

    private static AnimeCharacter ToAnimeCharacter(Guid animeId, Guid characterId)
    {
        return new AnimeCharacter
        {
            AnimeId = animeId,
            CharacterId = characterId
        };
    }
}
