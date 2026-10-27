using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Miru.Data.Seeding;
using Miru.DTO;
using Miru.Models;
using Miru.Tests.Support;
using NSubstitute;

namespace Miru.Tests.Integration.Controllers;

public class CharacterControllerIntegrationTests : IClassFixture<MiruFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client;

    private readonly MiruFactory _factory;

    public CharacterControllerIntegrationTests(MiruFactory factory)
    {
        _factory = factory;

        _factory.CharacterRepository.ClearReceivedCalls();

        _client = factory.CreateSecureClient();
    }

    private static void AssertMatches(Character expected, CharacterResponse actual)
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
    public async Task GetByIdAsync_WhenCharacterExists_ReturnsOkWithCharacter()
    {
        var character = CharacterSamples.CreateAll()[0];

        _factory.CharacterRepository.GetByIdAsync(character.Id).Returns(character);

        var response = await _client.GetAsync($"/characters/{character.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<CharacterResponse>(JsonOptions);

        Assert.NotNull(body);

        AssertMatches(character, body);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCharacterDoesNotExist_ReturnsNotFound()
    {
        _factory.CharacterRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Character?)null);

        var response = await _client.GetAsync($"/characters/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdIsNotAGuid_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/characters/not-a-guid");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsJsonContentType()
    {
        var character = CharacterSamples.CreateAll()[0];

        _factory.CharacterRepository.GetByIdAsync(character.Id).Returns(character);

        var response = await _client.GetAsync($"/characters/{character.Id}");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnceWithGivenId()
    {
        var id = Guid.NewGuid();

        _factory.CharacterRepository.GetByIdAsync(id).Returns((Character?)null);

        await _client.GetAsync($"/characters/{id}");

        await _factory.CharacterRepository.Received(1).GetByIdAsync(id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdIsNotAGuid_DoesNotCallRepository()
    {
        await _client.GetAsync("/characters/not-a-guid");

        await _factory.CharacterRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>());
    }
}
