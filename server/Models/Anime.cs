using Miru.Enums;

namespace Miru.Models;

public class Anime
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? JapaneseTitle { get; set; } = string.Empty;

    public string? Synopsis { get; set; } = string.Empty;

    public AnimeType Type { get; set; }

    public int? Episodes { get; set; }

    public AnimeStatus Status { get; set; }

    public DateOnly? AiredFrom { get; set; }

    public DateOnly? AiredTo { get; set; }

    public decimal? Score { get; set; }

    public string? ImageSource { get; set; } = string.Empty;

    public ICollection<AnimeCharacter> AnimeCharacters { get; set; } = new List<AnimeCharacter>();

    public ICollection<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();
}
