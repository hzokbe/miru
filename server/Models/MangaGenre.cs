namespace Miru.Models;

public class MangaGenre
{
    public Guid MangaId { get; set; }

    public Manga Manga { get; set; } = null!;

    public Guid GenreId { get; set; }

    public Genre Genre { get; set; } = null!;
}
