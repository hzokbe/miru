using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public static class MangaResponseFactory
{
    public static MangaResponse From(Manga manga)
    {
        return new MangaResponse(
            manga.Id,
            manga.Title,
            manga.JapaneseTitle,
            manga.Synopsis,
            manga.Chapters,
            manga.Volumes,
            manga.Status,
            manga.PublishedFrom,
            manga.PublishedTo,
            manga.Score,
            manga.ImageSource
        );
    }
}
