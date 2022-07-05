using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Calendar.WebService.Services;
using Kalantyr.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;

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

        private readonly IServiceProvider _serviceProvider;

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

        [HttpPost]
        [Route("migrate")]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MigrateAsync(CancellationToken cancellationToken)
        {
            var adminService = _serviceProvider.GetService<AdminService>();
            await adminService.MigrateAsync(Request.GetAuthToken(), cancellationToken);
            return Ok();
        }
    }
}
