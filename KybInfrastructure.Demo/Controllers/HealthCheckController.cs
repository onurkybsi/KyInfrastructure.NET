using Microsoft.AspNetCore.Mvc;

namespace KybInfrastructure.Demo.Controllers
{
    /// <summary>
    /// Endpoints which provide liveness info of the server
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Returns 200 if the server is running
        /// </summary>
        /// <returns></returns>
        public IActionResult CheckHealth()
            => Ok("I'm healty !");
    }
}
