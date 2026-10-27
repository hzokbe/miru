using Microsoft.AspNetCore.Mvc;
using Miru.Services;

namespace Miru.Controllers;

[ApiController]
[Route("/characters")]
public class CharacterController(ICharacterService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var character = await service.GetByIdAsync(id);

        return character != null ? Ok(character) : NotFound();
    }
}
