using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public static class AnimeAssert
{
    public static void Matches(Anime expected, AnimeResponse actual)
    {
        Assert.Equal(expected.Id, actual.Id);

        Assert.Equal(expected.Title, actual.Title);

        Assert.Equal(expected.JapaneseTitle, actual.JapaneseTitle);

        Assert.Equal(expected.Synopsis, actual.Synopsis);

        Assert.Equal(expected.Type, actual.Type);

        Assert.Equal(expected.Episodes, actual.Episodes);

        Assert.Equal(expected.Status, actual.Status);

        Assert.Equal(expected.AiredFrom, actual.AiredFrom);

        Assert.Equal(expected.AiredTo, actual.AiredTo);

        Assert.Equal(expected.Score, actual.Score);

        Assert.Equal(expected.ImageSource, actual.ImageSource);
    }

    public static void Matches(Anime expected, Anime actual)
    {
        Assert.Equal(expected.Id, actual.Id);

        Assert.Equal(expected.Title, actual.Title);

        Assert.Equal(expected.JapaneseTitle, actual.JapaneseTitle);

        Assert.Equal(expected.Synopsis, actual.Synopsis);

        Assert.Equal(expected.Type, actual.Type);

        Assert.Equal(expected.Episodes, actual.Episodes);

        Assert.Equal(expected.Status, actual.Status);

        Assert.Equal(expected.AiredFrom, actual.AiredFrom);

        Assert.Equal(expected.AiredTo, actual.AiredTo);

        Assert.Equal(expected.Score, actual.Score);

        Assert.Equal(expected.ImageSource, actual.ImageSource);
    }
}
