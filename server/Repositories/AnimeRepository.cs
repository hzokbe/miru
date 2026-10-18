using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;

namespace Miru.Repositories;

public class AnimeRepository(AppDbContext context) : IAnimeRepository
{
    public async Task<List<Anime>> GetAllAsync()
    {
        return await context.Animes.AsNoTracking().OrderByDescending(a => a.Score).ToListAsync();
    }
}
