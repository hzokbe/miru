namespace Miru.Models;

public class AnimeGenre
{
    public Guid AnimeId { get; set; }

    public Anime Anime { get; set; } = null!;

    public Guid GenreId { get; set; }

    public Genre Genre { get; set; } = null!;
}
