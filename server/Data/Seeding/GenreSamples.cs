using Miru.Models;

namespace Miru.Data.Seeding;

public static class GenreSamples
{
    public static List<Genre> CreateAll()
    {
        return
        [
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Action",
                Description = "Fast-paced stories driven by combat, chases and physical conflict."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Adventure",
                Description = "Journeys into unknown places, with exploration, risk and discovery."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Award Winning",
                Description = "Works recognized with notable awards or critical acclaim."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Comedy",
                Description = "Lighthearted stories built to make the audience laugh."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Drama",
                Description = "Emotionally driven stories focused on serious conflicts and character development."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Fantasy",
                Description = "Stories set in imaginary worlds featuring magic and supernatural elements."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Romance",
                Description = "Stories centered on love and the relationships between characters."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Sci-Fi",
                Description = "Stories exploring speculative science, technology and the future."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Suspense",
                Description = "Tense stories that build anticipation and uncertainty about what comes next."
            }
        ];
    }
}
