using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing application-related operations.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Application API")]
    [Route("api/app")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        /// <summary>
        /// Gets the version of the application.
        /// </summary>
        /// <returns>The version of the application.</returns>
        [HttpGet("version", Name = "GET Back Version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GetVersion()
        {
            return "1.2.0";
        }

    }
}
