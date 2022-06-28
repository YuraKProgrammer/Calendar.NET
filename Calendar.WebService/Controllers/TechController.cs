using Microsoft.AspNetCore.Mvc;

namespace Calendar.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechController : ControllerBase
    {
        [HttpGet]
        [Route("Version")]
        public async Task<IActionResult> GetVersionAsync(CancellationToken cancellationToken)
        {
            var v = typeof(TechController).Assembly.GetName().Version;
            return Ok(v.ToString());
        }
    }
}
