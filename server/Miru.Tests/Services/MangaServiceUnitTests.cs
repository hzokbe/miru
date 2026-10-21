using Miru.DTO;
using Miru.Enums;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;
using Moq;

namespace Miru.Tests.Services;

public class MangaServiceUnitTests
{
    private readonly Mock<IMangaRepository> _repository = new();

    private readonly MangaService _service;

    public MangaServiceUnitTests()
    {
        _service = new MangaService(_repository.Object);
    }

    private static Genre CreateGenre(string name, string? description = null)
    {
        return new Genre { Id = Guid.NewGuid(), Name = name, Description = description };
    }

    private static Character CreateCharacter(string name, CharacterRole role = CharacterRole.Main)
    {
        return new Character
        {
            Id = Guid.NewGuid(),
            Name = name,
            LastName = name + "Last",
            JapaneseName = name + "Jp",
            JapaneseLastName = name + "LastJp",
            Age = "17",
            Gender = CharacterGender.Female,
            Role = role,
            Description = name + " description.",
            ImageSource = "/images/character/" + name
        };
    }

    private static Manga CreateMangaWithGenres(Guid id, params Genre[] genres)
    {
        return new Manga
        {
            Id = id,
            MangaGenres = [.. genres.Select(g => new MangaGenre { MangaId = id, GenreId = g.Id, Genre = g })]
        };
    }

    private static Manga CreateMangaWithCharacters(Guid id, params Character[] characters)
    {
        return new Manga
        {
            Id = id,
            MangaCharacters =
            [
                .. characters.Select(c => new MangaCharacter { MangaId = id, CharacterId = c.Id, Character = c })
            ]
        };
    }

    private static void AssertCharacterMapped(Character expected, CharacterResponse actual)
    {
        Assert.Equal(expected.Id, actual.Id);

        Assert.Equal(expected.Name, actual.Name);

        Assert.Equal(expected.LastName, actual.LastName);

        Assert.Equal(expected.JapaneseName, actual.JapaneseName);

        Assert.Equal(expected.JapaneseLastName, actual.JapaneseLastName);

        Assert.Equal(expected.Age, actual.Age);

        Assert.Equal(expected.Gender, actual.Gender);

        Assert.Equal(expected.Role, actual.Role);

        Assert.Equal(expected.Description, actual.Description);

        Assert.Equal(expected.ImageSource, actual.ImageSource);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsNoMangas_ReturnsEmptyList()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_MapsAllFieldsCorrectly()
    {
        var id = Guid.NewGuid();

        var manga = new Manga
        {
            Id = id,
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            Synopsis =
                "At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.",
            Chapters = 281,
            Volumes = 28,
            Status = MangaStatus.Finished,
            PublishedFrom = new DateOnly(2015, 5, 19),
            PublishedTo = new DateOnly(2022, 11, 2),
            Score = 8.88m,
            ImageSource = "/images/manga/" + id
        };

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = await _service.GetAllAsync();

        var response = Assert.Single(result);

        Assert.Equal(manga.Id, response.Id);

        Assert.Equal(manga.Title, response.Title);

        Assert.Equal(manga.JapaneseTitle, response.JapaneseTitle);

        Assert.Equal(manga.Synopsis, response.Synopsis);

        Assert.Equal(manga.Chapters, response.Chapters);

        Assert.Equal(manga.Volumes, response.Volumes);

        Assert.Equal(manga.Status, response.Status);

        Assert.Equal(manga.PublishedFrom, response.PublishedFrom);

        Assert.Equal(manga.PublishedTo, response.PublishedTo);

        Assert.Equal(manga.Score, response.Score);

        Assert.Equal(manga.ImageSource, response.ImageSource);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        await _service.GetAllAsync();

        _repository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasNoGenres_ReturnsEmptyGenres()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([new Manga { Id = Guid.NewGuid() }]);

        var result = await _service.GetAllAsync();

        Assert.Empty(Assert.Single(result).Genres);
    }

    [Fact]
    public async Task GetAllAsync_MapsGenresCorrectly()
    {
        var drama = CreateGenre(
            "Drama",
            "Emotionally driven stories centered on realistic conflicts and character development."
        );

        var sliceOfLife = CreateGenre("Slice of Life", "Low-stakes stories about everyday moments and ordinary people.");

        var manga = CreateMangaWithGenres(Guid.NewGuid(), drama, sliceOfLife);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = await _service.GetAllAsync();

        var genres = Assert.Single(result).Genres;

        Assert.Equal(2, genres.Count);

        Assert.Equal(drama.Id, genres[0].Id);

        Assert.Equal(drama.Name, genres[0].Name);

        Assert.Equal(drama.Description, genres[0].Description);

        Assert.Equal(sliceOfLife.Id, genres[1].Id);

        Assert.Equal(sliceOfLife.Name, genres[1].Name);

        Assert.Equal(sliceOfLife.Description, genres[1].Description);
    }

    [Fact]
    public async Task GetAllAsync_MapsGenresForEachManga()
    {
        var comedy = CreateGenre("Comedy");

        var drama = CreateGenre("Drama");

        var first = CreateMangaWithGenres(Guid.NewGuid(), comedy);

        var second = CreateMangaWithGenres(Guid.NewGuid(), drama);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([first, second]);

        var result = await _service.GetAllAsync();

        Assert.Equal(["Comedy"], result[0].Genres.Select(g => g.Name));

        Assert.Equal(["Drama"], result[1].Genres.Select(g => g.Name));
    }

    [Fact]
    public async Task GetAllAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([new Manga { Id = Guid.NewGuid() }]);

        var result = await _service.GetAllAsync();

        Assert.Empty(Assert.Single(result).Characters);
    }

    [Fact]
    public async Task GetAllAsync_MapsCharactersCorrectly()
    {
        var kaguya = CreateCharacter("Kaguya");

        var miyuki = CreateCharacter("Miyuki", CharacterRole.Supporting);

        var manga = CreateMangaWithCharacters(Guid.NewGuid(), kaguya, miyuki);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([manga]);

        var result = await _service.GetAllAsync();

        var characters = Assert.Single(result).Characters;

        Assert.Equal(2, characters.Count);

        AssertCharacterMapped(kaguya, characters[0]);

        AssertCharacterMapped(miyuki, characters[1]);
    }

    [Fact]
    public async Task GetAllAsync_MapsCharactersForEachManga()
    {
        var punpun = CreateCharacter("Punpun");

        var yotsuba = CreateCharacter("Yotsuba");

        var first = CreateMangaWithCharacters(Guid.NewGuid(), punpun);

        var second = CreateMangaWithCharacters(Guid.NewGuid(), yotsuba);

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([first, second]);

        var result = await _service.GetAllAsync();

        Assert.Equal(["Punpun"], result[0].Characters.Select(c => c.Name));

        Assert.Equal(["Yotsuba"], result[1].Characters.Select(c => c.Name));
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Manga?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryReturnsManga_ReturnsManga()
    {
        var id = Guid.NewGuid();

        var manga = new Manga { Id = id };

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(manga.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFieldsCorrectly()
    {
        var id = Guid.NewGuid();

        var manga = new Manga
        {
            Id = id,
            Title = "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen",
            JapaneseTitle = "かぐや様は告らせたい～天才たちの恋愛頭脳戦～",
            Synopsis =
                "At an elite academy, the two top students of the student council are secretly attracted to each other. Too proud to confess, both scheme and bluff in elaborate mind games, each hoping the other will give in first.",
            Chapters = 281,
            Volumes = 28,
            Status = MangaStatus.Finished,
            PublishedFrom = new DateOnly(2015, 5, 19),
            PublishedTo = new DateOnly(2022, 11, 2),
            Score = 8.88m,
            ImageSource = "/images/manga/" + id
        };

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(manga);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(manga.Id, result.Id);

        Assert.Equal(manga.Title, result.Title);

        Assert.Equal(manga.JapaneseTitle, result.JapaneseTitle);

        Assert.Equal(manga.Synopsis, result.Synopsis);

        Assert.Equal(manga.Chapters, result.Chapters);

        Assert.Equal(manga.Volumes, result.Volumes);

        Assert.Equal(manga.Status, result.Status);

        Assert.Equal(manga.PublishedFrom, result.PublishedFrom);

        Assert.Equal(manga.PublishedTo, result.PublishedTo);

        Assert.Equal(manga.Score, result.Score);

        Assert.Equal(manga.ImageSource, result.ImageSource);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnce()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Manga?)null);

        await _service.GetByIdAsync(Guid.NewGuid());

        _repository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoGenres_ReturnsEmptyGenres()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Manga { Id = id });

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Empty(result.Genres);
    }

    [Fact]
    public async Task GetByIdAsync_MapsGenresCorrectly()
    {
        var id = Guid.NewGuid();

        var drama = CreateGenre(
            "Drama",
            "Emotionally driven stories centered on realistic conflicts and character development."
        );

        var fantasy = CreateGenre(
            "Fantasy",
            "Stories set in worlds with magic, mythical creatures and supernatural elements."
        );

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CreateMangaWithGenres(id, drama, fantasy));

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(2, result.Genres.Count);

        Assert.Equal(drama.Id, result.Genres[0].Id);

        Assert.Equal(drama.Name, result.Genres[0].Name);

        Assert.Equal(drama.Description, result.Genres[0].Description);

        Assert.Equal(fantasy.Id, result.Genres[1].Id);

        Assert.Equal(fantasy.Name, result.Genres[1].Name);

        Assert.Equal(fantasy.Description, result.Genres[1].Description);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMangaHasNoCharacters_ReturnsEmptyCharacters()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Manga { Id = id });

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Empty(result.Characters);
    }

    [Fact]
    public async Task GetByIdAsync_MapsCharactersCorrectly()
    {
        var id = Guid.NewGuid();

        var musashi = CreateCharacter("Musashi");

        var matahachi = CreateCharacter("Matahachi", CharacterRole.Supporting);

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CreateMangaWithCharacters(id, musashi, matahachi));

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);

        Assert.Equal(2, result.Characters.Count);

        AssertCharacterMapped(musashi, result.Characters[0]);

        AssertCharacterMapped(matahachi, result.Characters[1]);
    }
}
