using Miru.DTO;

namespace Miru.Services;

public interface IAnimeService
{
    public Task<List<AnimeResponse>> GetAllAsync();

    public Task<AnimeResponse?> GetByIdAsync(Guid id);
}
