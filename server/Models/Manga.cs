using Miru.Enums;

namespace Miru.Models;

public class Manga
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? JapaneseTitle { get; set; } = string.Empty;

    public string? Synopsis { get; set; } = string.Empty;

    public int? Chapters { get; set; }

    public int? Volumes { get; set; }

    public MangaStatus Status { get; set; }

    public DateOnly? PublishedFrom { get; set; }

    public DateOnly? PublishedTo { get; set; }

    public decimal? Score { get; set; }

    public string? ImageSource { get; set; } = string.Empty;

    public ICollection<MangaCharacter> MangaCharacters { get; set; } = new List<MangaCharacter>();
}
