using Miru.DTO;

namespace Miru.Services;

public interface IMangaService
{
    public Task<List<MangaResponse>> GetAllAsync();

    public Task<MangaResponse?> GetByIdAsync(Guid id);
}
