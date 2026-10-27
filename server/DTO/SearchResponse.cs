namespace Miru.DTO;

public record SearchResponse(
    List<AnimeResponse> Animes,
    List<MangaResponse> Mangas,
    List<CharacterResponse> Characters
);
