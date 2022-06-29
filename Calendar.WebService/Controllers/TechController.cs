using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace Calendar.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechController : ControllerBase
    {
        public TechController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        IServiceProvider _serviceProvider;

        [HttpGet]
        [Route("Version")]
        public async Task<IActionResult> GetVersionAsync(CancellationToken cancellationToken)
        {
            var v = typeof(TechController).Assembly.GetName().Version;
            return Ok(v.ToString());
        }

        [HttpGet]
        [Route("selfTest")]
        public async Task<IActionResult> SelfTestAsync(CancellationToken cancellationToken)
        {
            foreach (var healthCheck in _serviceProvider.GetServices<IHealthCheck>())
            {
                var result = await healthCheck.CheckHealthAsync(new HealthCheckContext(), cancellationToken);
                if (result.Status != HealthStatus.Healthy)
                    return Problem(result.Exception.GetBaseException().Message, null, (int)HttpStatusCode.InternalServerError);
            }

            return Ok("Success");
        }
    }
}
