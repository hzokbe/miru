using Microsoft.AspNetCore.Mvc;
using Miru.DTO;
using Miru.Services;

namespace Miru.Controllers;

[ApiController]
[Route("/mangas")]
public class MangaController(IMangaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MangaResponse>>> GetAllAsync()
    {
        return Ok(await service.GetAllAsync());
    }
}
