using Miru.DTO;

namespace Miru.Services;

public interface ICharacterService
{
    public Task<CharacterResponse?> GetByIdAsync(Guid id);
}
