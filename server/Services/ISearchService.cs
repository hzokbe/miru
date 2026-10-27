using Miru.DTO;

namespace Miru.Services;

public interface ISearchService
{
    public Task<SearchResponse> SearchAsync(string query);
}
