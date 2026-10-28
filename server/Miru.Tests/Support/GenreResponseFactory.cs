using Miru.DTO;
using Miru.Models;

namespace Miru.Tests.Support;

public class GenreResponseFactory
{
    public static GenreResponse From(Genre genre)
    {
        return new GenreResponse(
            genre.Id,
            genre.Name,
            genre.Description
        );
    }
}
