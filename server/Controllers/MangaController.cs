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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var manga = await service.GetByIdAsync(id);

        return manga != null ? Ok(manga) : NotFound();
    }
}
