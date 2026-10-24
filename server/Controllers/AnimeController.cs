using Microsoft.AspNetCore.Mvc;
using Miru.DTO;
using Miru.Services;

namespace Miru.Controllers;

[ApiController]
[Route("/animes")]
public class AnimeController(IAnimeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AnimeResponse>>> GetAllAsync()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var anime = await service.GetByIdAsync(id);

        return anime != null ? Ok(anime) : NotFound();
    }
}
