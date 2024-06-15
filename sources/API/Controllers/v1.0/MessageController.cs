using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to messages.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Message APIs")]
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="messageService">The message service.</param>
        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Gets all Message with pages.
        /// </summary>
        /// <returns>A list of all Message.</returns>
        [HttpGet("conversation/{conversationId}/pages", Name = "GET - Entrypoint for get all Messages with pages for one conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMessagesWithPagesForConversation(
            int conversationId,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0)
        {
            var messages = _messageService.GetAllMessagesWithPagesForConversation(conversationId, pageSize, pageNumber);
            return Ok(messages);
        }

        /// <summary>
        /// Gets a message by its identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>The message with the specified identifier.</returns>
        [HttpGet("{messageId}", Name = "GET - Entrypoint for get Message by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMessageById(int messageId)
        {
            var message = _messageService.GetMessageById(messageId);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
    }
}
