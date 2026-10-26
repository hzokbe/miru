namespace Miru.Models;

public class MangaCharacter
{
    public Guid MangaId { get; set; }

    public Manga Manga { get; set; } = null!;

    public Guid CharacterId { get; set; }

    public Character Character { get; set; } = null!;
}
