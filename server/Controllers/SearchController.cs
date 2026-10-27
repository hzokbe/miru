using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Miru.DTO;
using Miru.Services;

namespace Miru.Controllers;

[ApiController]
[Route("/search")]
public class SearchController(ISearchService service) : ControllerBase
{
    [HttpGet("{query}")]
    public async Task<ActionResult<SearchResponse>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest();

        return Ok(await service.SearchAsync(query));
    }
}
