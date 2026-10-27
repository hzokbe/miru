using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;

namespace Miru.Repositories;

public class MangaRepository(AppDbContext context) : IMangaRepository
{
    public async Task<List<Manga>> GetAllAsync()
    {
        return await context
            .Mangas
            .AsNoTracking()
            .Include(m => m.MangaCharacters)
            .ThenInclude(mc => mc.Character)
            .OrderByDescending(m => m.Score)
            .ToListAsync();
    }

    public async Task<Manga?> GetByIdAsync(Guid id)
    {
        return await context
            .Mangas
            .Include(m => m.MangaCharacters)
            .ThenInclude(mc => mc.Character)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Manga>> SearchAsync(string query)
    {
        return await context
            .Mangas
            .AsNoTracking()
            .Where(m => m.Title.ToLower().Contains(query.ToLower()))
            .OrderByDescending(m => m.Score)
            .ToListAsync();
    }
}
