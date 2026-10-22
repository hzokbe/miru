using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Miru.DTO;

namespace Miru.Controllers;

[ApiController]
[Route("/health")]
public class HealthController(HealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<HealthResponse>> GetHealth(CancellationToken cancellationToken)
    {
        var report = await healthCheckService.CheckHealthAsync(cancellationToken);

        var response = new HealthResponse(report.Status.ToString());

        return report.Status == HealthStatus.Healthy ? Ok(response) : StatusCode(503, response);
    }
}
