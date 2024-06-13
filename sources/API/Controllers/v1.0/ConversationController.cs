using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to conversations.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Conversation APIs")]
    [ApiController]
    [Route("api/conversations")]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly ConversationService _conversationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationController"/> class.
        /// </summary>
        /// <param name="conversationService">The conversation service.</param>
        public ConversationController(ConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        /// <summary>
        /// Creates a new conversation.
        /// </summary>
        /// <param name="conversation">The conversation to create.</param>
        /// <returns>The newly created conversation.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateConversation([FromBody] Conversation conversation)
        {
            var createdConversation = _conversationService.CreateConversation(conversation);
            return CreatedAtAction(nameof(GetConversationById), new { conversationId = createdConversation.Id }, createdConversation);
        }

        /// <summary>
        /// Gets all conversations.
        /// </summary>
        /// <returns>A list of all conversations.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Conversations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllConversations()
        {
            var conversations = _conversationService.GetAllConversations();
            return Ok(conversations);
        }

        /// <summary>
        /// Gets all Conversations with pages.
        /// </summary>
        /// <returns>A list of all Conversations.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all Conversations with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllConversationsWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0,
            bool includeProfiles = false,
            bool includeMessages = false)
        {
            var conversations = _conversationService.GetAllConversationsWithPages(pageSize, pageNumber, includeProfiles, includeMessages);
            return Ok(conversations);
        }

        /// <summary>
        /// Gets all Conversations with pages.
        /// </summary>
        /// <returns>A list of all Conversations.</returns>
        [HttpGet("user/{userId}", Name = "GET - Entrypoint for get user Conversations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetConversationsForUser(int userId, bool includeProfiles = false, bool includeMessages = false)
        {
            var conversations = _conversationService.GetConversationsForUser(userId, includeProfiles, includeMessages);
            return Ok(conversations);
        }

        /// <summary>
        /// Gets a conversation by its identifier.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The conversation with the specified identifier.</returns>
        [HttpGet("{conversationId}", Name = "GET - Entrypoint for get Conversation by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetConversationById(int conversationId)
        {
            var conversation = _conversationService.GetConversationById(conversationId);

            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(conversation);
        }

        /// <summary>
        /// Deletes a conversation by its identifier.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns>The deleted conversation.</returns>
        [HttpDelete("{conversationId}", Name = "DELETE - Entrypoint for remove Conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteConversation(int conversationId)
        {
            try
            {
                var deletedConversation = _conversationService.DeleteConversation(conversationId);
                return Ok(deletedConversation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Add a Profile in conversation.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>The updated profile.</returns>
        [HttpPost("{conversationId}/add/user/{profileId}", Name = "POST - Entrypoint for add Profile in conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMemberInConversation(int conversationId, int profileId)
        {
            try
            {
                return Ok(await _conversationService.AddMemberToConversation(conversationId, profileId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Remove a Profile in conversation.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>The updated profile.</returns>
        [HttpDelete("{conversationId}/remove/user/{profileId}", Name = "DELETE - Entrypoint for add Profile in conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveMemberInConversation(int conversationId, int profileId)
        {
            try
            {
                return Ok(await _conversationService.RemoveMemberToConversation(conversationId, profileId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
