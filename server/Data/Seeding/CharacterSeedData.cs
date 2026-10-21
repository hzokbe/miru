using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public static class CharacterSeedData
{
    public static List<Character> Create()
    {
        return
        [
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Frieren",
                JapaneseName = "フリーレン",
                Age = "1000+",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A long-lived elf mage who defeated the Demon King and now travels to understand humans and her fallen companions."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Fern",
                JapaneseName = "フェルン",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "Heiter's adopted daughter and Frieren's apprentice, serious and mature, who looks after the party like a mother."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Stark",
                JapaneseName = "シュタルク",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description = "A carefree and cowardly young warrior who overcomes his fear to protect those he loves."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Edward",
                LastName = "Elric",
                JapaneseName = "エドワード",
                JapaneseLastName = "エルリック",
                Age = "15-16",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A prodigy and the youngest State Alchemist, determined to restore his brother's body after a failed transmutation."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Alphonse",
                LastName = "Elric",
                JapaneseName = "アルフォンス",
                JapaneseLastName = "エルリック",
                Age = "14-15",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description = "Edward's younger brother, whose soul is bound to a suit of armor. Kind and gentle."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Rintarou",
                LastName = "Okabe",
                JapaneseName = "倫太郎",
                JapaneseLastName = "岡部",
                Age = "18",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A theatrical, self-proclaimed mad scientist and founder of the Future Gadget Lab."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kurisu",
                LastName = "Makise",
                JapaneseName = "紅莉栖",
                JapaneseLastName = "牧瀬",
                Age = "18",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description = "A brilliant young neuroscience researcher, curious and proud, with a tsundere streak."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Mayuri",
                LastName = "Shiina",
                JapaneseName = "まゆり",
                JapaneseLastName = "椎名",
                Age = "16",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description = "Okabe's cheerful, optimistic childhood friend, who always seems slow and dreamy."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kaguya",
                LastName = "Shinomiya",
                JapaneseName = "かぐや",
                JapaneseLastName = "四宮",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "The student council vice president and heiress of the Shinomiya group, gifted but sheltered and proud."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Miyuki",
                LastName = "Shirogane",
                JapaneseName = "御行",
                JapaneseLastName = "白銀",
                Age = "17",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "The hardworking student council president, who refuses to confess his love for Kaguya first."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Denji",
                JapaneseName = "デンジ",
                Age = "16",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A young man in debt who merges with the devil Pochita and becomes a devil hunter."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Power",
                JapaneseName = "パワー",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A blood fiend and Public Safety Devil Hunter, rude and dishonest but devoted to her cat Nyako."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Makima",
                JapaneseName = "マキマ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description = "A Public Safety Devil Hunter who leads Denji and Power's squad."
            }
        ];
    }
}
