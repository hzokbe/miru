using Miru.Enums;

namespace Miru.Models;

public class Character
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string? JapaneseName { get; set; }

    public string? JapaneseLastName { get; set; }

    public string? Age { get; set; }

    public CharacterGender Gender { get; set; }

    public CharacterRole Role { get; set; }

    public string? Description { get; set; }

    public string? ImageSource { get; set; }

    public ICollection<AnimeCharacter> AnimeCharacters { get; set; } = new List<AnimeCharacter>();

    public ICollection<MangaCharacter> MangaCharacters { get; set; } = new List<MangaCharacter>();
}
