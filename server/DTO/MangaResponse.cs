using Miru.Enums;

namespace Miru.DTO;

public record MangaResponse(
    Guid Id,
    string Title,
    string? JapaneseTitle,
    string? Synopsis,
    int? Chapters,
    int? Volumes,
    MangaStatus Status,
    DateOnly? PublishedFrom,
    DateOnly? PublishedTo,
    decimal? Score,
    string? ImageSource
);
