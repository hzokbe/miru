using Miru.DTO;
using Miru.Models;
using Miru.Repositories;

namespace Miru.Services;

public class MangaService(IMangaRepository repository) : IMangaService
{
    public async Task<List<MangaResponse>> GetAllAsync()
    {
        return [.. (await repository.GetAllAsync()).Select(ToResponse)];
    }

    public async Task<MangaResponse?> GetByIdAsync(Guid id)
    {
        var manga = await repository.GetByIdAsync(id);

        return manga != null ? ToResponse(manga) : null;
    }

    private static MangaResponse ToResponse(Manga manga)
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
            manga.ImageSource,
            [.. manga.MangaCharacters.Select(mc => ToResponse(mc.Character))]
        );
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
