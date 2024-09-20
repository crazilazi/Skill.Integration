using Microsoft.AspNetCore.Mvc;

namespace Skill.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        // Basic health check to verify the app is running
        [HttpGet]
        public IActionResult Get()
        {
            // Optionally add more checks like memory, uptime, etc.
            var healthCheckResult = new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                uptime = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()
            };

            return Ok(healthCheckResult);
        }
    }
}
