using Miru.Enums;

namespace Miru.DTO;

public record AnimeResponse(
    Guid Id,
    string Title,
    string? JapaneseTitle,
    string? Synopsis,
    AnimeType Type,
    int? Episodes,
    AnimeStatus Status,
    DateOnly? AiredFrom,
    DateOnly? AiredTo,
    decimal? Score,
    string? ImageSource,
    List<GenreResponse> Genres,
    List<CharacterResponse> Characters
);
