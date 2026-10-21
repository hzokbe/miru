namespace Miru.Models;

public class AnimeCharacter
{
    public Guid AnimeId { get; set; }

    public Anime Anime { get; set; } = null!;

    public Guid CharacterId { get; set; }

    public Character Character { get; set; } = null!;
}
