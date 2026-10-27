using Miru.DTO;
using Miru.Models;
using Miru.Repositories;

namespace Miru.Services;

public class CharacterService(ICharacterRepository repository) : ICharacterService
{
    public async Task<CharacterResponse?> GetByIdAsync(Guid id)
    {
        var character = await repository.GetByIdAsync(id);

        return character != null ? ToResponse(character) : null;
    }

    private static CharacterResponse ToResponse(Character character)
    {
        return new CharacterResponse(
            character.Id,
            character.Name,
            character.LastName,
            character.JapaneseName,
            character.JapaneseLastName,
            character.Age,
            character.Gender,
            character.Role,
            character.Description,
            character.ImageSource
        );
    }
}
