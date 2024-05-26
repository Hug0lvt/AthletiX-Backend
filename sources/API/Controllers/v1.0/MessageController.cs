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
        /// Creates a new message.
        /// </summary>
        /// <param name="message">The message to create.</param>
        /// <returns>The newly created message.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            var createdMessage = _messageService.CreateMessage(message);
            return CreatedAtAction(nameof(GetMessageById), new { messageId = createdMessage.Id }, createdMessage);
        }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>A list of all messages.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMessages()
        {
            var messages = _messageService.GetAllMessages();
            return Ok(messages);
        }

        /// <summary>
        /// Gets all Message with pages.
        /// </summary>
        /// <returns>A list of all Message.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all Messages with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMessagesWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0)
        {
            var messages = _messageService.GetAllMessagesWithPages(pageSize, pageNumber);
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

        /// <summary>
        /// Updates a message.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="updatedMessage">The updated message information.</param>
        /// <returns>The updated message.</returns>
        [HttpPut("{messageId}", Name = "PUT - Entrypoint for update Message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateMessage(int messageId, [FromBody] Message updatedMessage)
        {
            try
            {
                if(updatedMessage.Id != messageId) updatedMessage.Id = messageId;
                var message = _messageService.UpdateMessage(updatedMessage);
                return Ok(message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a message by its identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>The deleted message.</returns>
        [HttpDelete("{messageId}", Name = "DELETE - Entrypoint for remove Message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMessage(int messageId)
        {
            try
            {
                var deletedMessage = _messageService.DeleteMessage(messageId);
                return Ok(deletedMessage);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
