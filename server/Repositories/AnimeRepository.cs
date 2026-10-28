using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;

namespace Miru.Repositories;

public class AnimeRepository(AppDbContext context) : IAnimeRepository
{
    public async Task<List<Anime>> GetAllAsync()
    {
        return await context
            .Animes
            .AsNoTracking()
            .Include(a => a.AnimeCharacters
                .OrderBy(ac => ac.Character.Role)
                .ThenBy(ac => ac.Character.Name)
            )
            .ThenInclude(ac => ac.Character)
            .Include(a => a.AnimeGenres.OrderBy(ag => ag.Genre.Name))
            .ThenInclude(ac => ac.Genre)
            .OrderByDescending(a => a.Score)
            .ToListAsync();
    }

    public async Task<Anime?> GetByIdAsync(Guid id)
    {
        return await context
            .Animes
            .Include(a => a.AnimeCharacters
                .OrderBy(ac => ac.Character.Role)
                .ThenBy(ac => ac.Character.Name)
            )
            .ThenInclude(ac => ac.Character)
            .Include(a => a.AnimeGenres.OrderBy(ag => ag.Genre.Name))
            .ThenInclude(ac => ac.Genre)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Anime>> SearchAsync(string query)
    {
        return await context
            .Animes
            .AsNoTracking()
            .Where(a => a.Title.ToLower().Contains(query.ToLower()))
            .OrderByDescending(a => a.Score)
            .ToListAsync();
    }
}
