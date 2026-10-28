namespace Miru.Models;

public class Genre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();
}
