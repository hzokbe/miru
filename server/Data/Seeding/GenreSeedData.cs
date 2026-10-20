using Miru.Models;

namespace Miru.Data.Seeding;

public static class GenreSeedData
{
    public static List<Genre> Create()
    {
        return
        [
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Action",
                Description = "Fast-paced stories driven by combat, chases and high-stakes physical conflict."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Adventure",
                Description = "Journeys and quests through unfamiliar places, full of exploration and risk."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Comedy",
                Description =
                    "Humor-focused stories meant to entertain through jokes, absurd situations and witty banter."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Drama",
                Description = "Emotionally driven stories centered on realistic conflicts and character development."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Fantasy",
                Description = "Stories set in worlds with magic, mythical creatures and supernatural elements."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Romance",
                Description = "Stories centered on love, attraction and the development of romantic relationships."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Sci-Fi",
                Description =
                    "Stories built on speculative science and technology, such as time travel, AI and the future."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Slice of Life",
                Description = "Low-stakes stories about everyday moments and ordinary people."
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Suspense",
                Description = "Tense stories that keep the audience anxious about what will happen next."
            }
        ];
    }
}
