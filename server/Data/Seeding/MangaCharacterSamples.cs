using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class MangaCharacterSamples
{
    private const string Vagabond = "Vagabond";

    private const string YotsubaTo = "Yotsuba to!";

    private const string KaguyaSama = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen";

    private const string KaoruHanaWaRinToSaku = "Kaoru Hana wa Rin to Saku";

    private const string SousouNoFrieren = "Sousou no Frieren";

    private const string OyasumiPunpun = "Oyasumi Punpun";

    private const string ShoujoShuumatsuRyokou = "Shoujo Shuumatsu Ryokou";

    private const string ChainsawMan = "Chainsaw Man";

    private const string ShimejiSimulation = "Shimeji Simulation";

    public static List<MangaCharacter> CreateAll(List<Manga> mangas, List<Character> characters)
    {
        var mangaByTitle = mangas.ToDictionary(manga => manga.Title);

        var characterByFullName = characters.ToDictionary(ToFullName);

        var links = new (string Manga, string Character, CharacterRole Role)[]
        {
            (Vagabond, "Musashi Miyamoto", CharacterRole.Main),
            (Vagabond, "Kojirou Sasaki", CharacterRole.Main),
            (Vagabond, "Matahachi Honiden", CharacterRole.Main),

            (YotsubaTo, "Yotsuba Koiwai", CharacterRole.Main),
            (YotsubaTo, "Yousuke Koiwai", CharacterRole.Main),
            (YotsubaTo, "Fuuka Ayase", CharacterRole.Main),
            (YotsubaTo, "Asagi Ayase", CharacterRole.Supporting),
            (YotsubaTo, "Takashi Takeda", CharacterRole.Supporting),

            (KaguyaSama, "Kaguya Shinomiya", CharacterRole.Main),
            (KaguyaSama, "Miyuki Shirogane", CharacterRole.Main),
            (KaguyaSama, "Miko Iino", CharacterRole.Main),
            (KaguyaSama, "Ai Hayasaka", CharacterRole.Supporting),
            (KaguyaSama, "Kei Shirogane", CharacterRole.Supporting),

            (KaoruHanaWaRinToSaku, "Kaoruko Waguri", CharacterRole.Main),
            (KaoruHanaWaRinToSaku, "Rintarou Tsumugi", CharacterRole.Main),
            (KaoruHanaWaRinToSaku, "Subaru Hoshina", CharacterRole.Supporting),
            (KaoruHanaWaRinToSaku, "Madoka Yuzuhara", CharacterRole.Supporting),

            (SousouNoFrieren, "Frieren", CharacterRole.Main),
            (SousouNoFrieren, "Fern", CharacterRole.Main),
            (SousouNoFrieren, "Stark", CharacterRole.Main),
            (SousouNoFrieren, "Himmel", CharacterRole.Supporting),
            (SousouNoFrieren, "Eisen", CharacterRole.Supporting),
            (SousouNoFrieren, "Heiter", CharacterRole.Supporting),

            (OyasumiPunpun, "Punpun Onodera", CharacterRole.Main),
            (OyasumiPunpun, "Aiko Tanaka", CharacterRole.Main),
            (OyasumiPunpun, "Sachi Nanjou", CharacterRole.Supporting),
            (OyasumiPunpun, "Masumi Seki", CharacterRole.Supporting),

            (ShoujoShuumatsuRyokou, "Chito", CharacterRole.Main),
            (ShoujoShuumatsuRyokou, "Yuuri", CharacterRole.Main),
            (ShoujoShuumatsuRyokou, "Nuko", CharacterRole.Supporting),
            (ShoujoShuumatsuRyokou, "Ishii", CharacterRole.Supporting),

            (ChainsawMan, "Denji", CharacterRole.Main),
            (ChainsawMan, "Power", CharacterRole.Main),
            (ChainsawMan, "Makima", CharacterRole.Main),
            (ChainsawMan, "Aki Hayakawa", CharacterRole.Main),
            (ChainsawMan, "Reze", CharacterRole.Supporting),
            (ChainsawMan, "Kobeni Higashiyama", CharacterRole.Supporting),

            (ShimejiSimulation, "Shijima Tsukishima", CharacterRole.Main),
            (ShimejiSimulation, "Majime Yamashita", CharacterRole.Main),
            (ShimejiSimulation, "Shijima's Older Sister", CharacterRole.Supporting),
            (ShimejiSimulation, "Mogawa-sensei", CharacterRole.Supporting)
        };

        return
        [
            .. links.Select(mc => ToMangaCharacter(mangaByTitle[mc.Manga].Id, characterByFullName[mc.Character].Id))
        ];
    }

    private static string ToFullName(Character character)
    {
        return string.IsNullOrEmpty(character.LastName) ? character.Name : $"{character.Name} {character.LastName}";
    }

    private static MangaCharacter ToMangaCharacter(Guid mangaId, Guid characterId)
    {
        return new MangaCharacter
        {
            MangaId = mangaId,
            CharacterId = characterId
        };
    }
}
