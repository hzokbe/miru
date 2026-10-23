using Miru.Models;

namespace Miru.Repositories;

public interface IAnimeRepository
{
    public Task<List<Anime>> GetAllAsync();
}
