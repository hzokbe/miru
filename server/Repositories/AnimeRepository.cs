using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;

namespace Miru.Repositories;

public class AnimeRepository(AppDbContext context) : IAnimeRepository
{
    public async Task<List<Anime>> GetAllAsync()
    {
        return await context.Animes.AsNoTracking().Include(a => a.AnimeGenres).ThenInclude(ag => ag.Genre)
            .OrderByDescending(a => a.Score).ToListAsync();
    }

    public async Task<Anime?> GetByIdAsync(Guid id)
    {
        return await context.Animes.Include(a => a.AnimeGenres).ThenInclude(ag => ag.Genre)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
