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
}
