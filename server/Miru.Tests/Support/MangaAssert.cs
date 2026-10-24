using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public static class MangaAssert
{
    public static void Matches(Manga expected, MangaResponse actual)
    {
        Assert.Equal(expected.Id, actual.Id);

        Assert.Equal(expected.Title, actual.Title);

        Assert.Equal(expected.JapaneseTitle, actual.JapaneseTitle);

        Assert.Equal(expected.Synopsis, actual.Synopsis);

        Assert.Equal(expected.Chapters, actual.Chapters);

        Assert.Equal(expected.Volumes, actual.Volumes);

        Assert.Equal(expected.Status, actual.Status);

        Assert.Equal(expected.PublishedFrom, actual.PublishedFrom);

        Assert.Equal(expected.PublishedTo, actual.PublishedTo);

        Assert.Equal(expected.Score, actual.Score);

        Assert.Equal(expected.ImageSource, actual.ImageSource);
    }
}
