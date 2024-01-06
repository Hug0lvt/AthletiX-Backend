using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Application API")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {

        [HttpGet(Name = "GET Back Version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GetVersion()
        {
            return "1.1.0";
        }

    }
}
