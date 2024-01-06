using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Session APIs")]
    [ApiController]
    [Route("api/sessions")]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateSession([FromBody] Session session)
        {
            var createdSession = _sessionService.CreateSession(session);
            return CreatedAtAction(nameof(GetSessionById), new { sessionId = createdSession.Id }, createdSession);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSessions()
        {
            var sessions = _sessionService.GetAllSessions();
            return Ok(sessions);
        }

        [HttpGet("{sessionId}", Name = "GET - Entrypoint for get Session by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSessionById(int sessionId)
        {
            var session = _sessionService.GetSessionById(sessionId);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        [HttpPut("{sessionId}", Name = "PUT - Entrypoint for update Session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSession(int sessionId, [FromBody] Session updatedSession)
        {
            try
            {
                var session = _sessionService.UpdateSession(updatedSession);

                return Ok(session);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{sessionId}", Name = "DELETE - Entrypoint for remove Session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSession(int sessionId)
        {
            try
            {
                var deletedSession = _sessionService.DeleteSession(sessionId);

                return Ok(deletedSession);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}