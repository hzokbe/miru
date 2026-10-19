using Miru.Models;

namespace Miru.Repositories;

public interface IMangaRepository
{
    public Task<List<Manga>> GetAllAsync();
}
