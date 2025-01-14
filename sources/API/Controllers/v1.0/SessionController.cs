﻿using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to sessions.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Session APIs")]
    [ApiController]
    [Route("api/sessions")]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionController"/> class.
        /// </summary>
        /// <param name="sessionService">The session service.</param>
        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="session">The session to create.</param>
        /// <returns>The newly created session.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSession([FromBody] Session session)
        {
            var createdSession = await _sessionService.CreateSessionAsync(session);
            return CreatedAtAction(nameof(GetSessionById), new { sessionId = createdSession.Id }, createdSession);
        }

        /// <summary>
        /// Gets all sessions.
        /// </summary>
        /// <returns>A list of all sessions.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSessions()
        {
            var sessions = _sessionService.GetAllSessions();
            return Ok(sessions);
        }

        /// <summary>
        /// Gets all Session with pages.
        /// </summary>
        /// <returns>A list of all Session.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all Sessions with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSessionsWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0,
            bool includeExercise = false)
        {
            var categories = _sessionService.GetAllSessionsWithPages(pageSize, pageNumber, includeExercise);
            return Ok(categories);
        }

        /// <summary>
        /// Gets user Session with pages.
        /// </summary>
        /// <returns>A list of all Session.</returns>
        [HttpGet("user/{userId}", Name = "GET - Entrypoint for get user Sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSessionsWithPages(int userId, bool includeExercise = false)
        {
            var sessions = _sessionService.GetSessionsFromUser(userId, includeExercise);
            return Ok(sessions);
        }

        /// <summary>
        /// Gets a session by its identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The session with the specified identifier.</returns>
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

        /// <summary>
        /// Updates a session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="updatedSession">The updated session information.</param>
        /// <returns>The updated session.</returns>
        [HttpPut("{sessionId}", Name = "PUT - Entrypoint for update Session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSession(int sessionId, [FromBody] Session updatedSession)
        {
            try
            {
                if(updatedSession.Id != sessionId) updatedSession.Id = sessionId;
                var session = _sessionService.UpdateSession(updatedSession);
                return Ok(session);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a session by its identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The deleted session.</returns>
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
        
        /// <summary>
        /// Updates status of an existing session.
        /// </summary>
        [HttpPut("{sessionId}/updateStatus/{status}", Name = "PUT - Entrypoint for update an status of Session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult UpdateStatusSession(int sessionId, int status)
        {
            try
            {
                var session = _sessionService.UpdateStatusSession(sessionId, status);
                return Ok(session);
            }
            catch (ConflictExecption conflictExecption)
            {
                return Conflict(conflictExecption.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
