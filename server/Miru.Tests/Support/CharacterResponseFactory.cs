using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public static class CharacterResponseFactory
{
    public static CharacterResponse From(Character character)
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
