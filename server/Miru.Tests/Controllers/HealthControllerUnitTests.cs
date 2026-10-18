using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Miru.Controllers;
using Miru.DTO;

namespace Miru.Tests.Controllers;

public class HealthControllerUnitTests
{
    private static HealthController CreateController(HealthStatus status)
    {
        var report = new HealthReport(
            new Dictionary<string, HealthReportEntry>(),
            status,
            TimeSpan.Zero
        );

        var serviceMock = new Mock<HealthCheckService>();

        serviceMock.Setup(x =>
                x.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);

        return new HealthController(serviceMock.Object);
    }

    [Fact]
    public async Task GetHealth_WhenHealthy_ReturnsOk()
    {
        var controller = CreateController(HealthStatus.Healthy);

        var result = await controller.GetHealth(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var response = Assert.IsType<HealthResponse>(okResult.Value);

        Assert.Equal("Healthy", response.Status);
    }

    [Theory]
    [InlineData(HealthStatus.Degraded, "Degraded")]
    [InlineData(HealthStatus.Unhealthy, "Unhealthy")]
    public async Task GetHealth_WhenNotHealthy_Returns503(HealthStatus status, string expectedStatus)
    {
        var controller = CreateController(status);

        var result = await controller.GetHealth(CancellationToken.None);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);

        Assert.Equal(503, objectResult.StatusCode);

        var response = Assert.IsType<HealthResponse>(objectResult.Value);

        Assert.Equal(expectedStatus, response.Status);
    }
}
