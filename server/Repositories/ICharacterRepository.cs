using Miru.Models;

namespace Miru.Repositories;

public interface ICharacterRepository
{
    public Task<Character?> GetByIdAsync(Guid id);
}
