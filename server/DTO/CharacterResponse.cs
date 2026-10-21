using Miru.Enums;

namespace Miru.DTO;

public record CharacterResponse(
    Guid Id,
    string Name,
    string? LastName,
    string? JapaneseName,
    string? JapaneseLastName,
    string? Age,
    CharacterGender Gender,
    CharacterRole Role,
    string? Description,
    string? ImageSource
);
