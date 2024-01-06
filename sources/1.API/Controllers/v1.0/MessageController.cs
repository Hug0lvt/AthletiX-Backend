using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Message APIs")]
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            var createdMessage = _messageService.CreateMessage(message);
            return CreatedAtAction(nameof(GetMessageById), new { messageId = createdMessage.Id }, createdMessage);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMessages()
        {
            var messages = _messageService.GetAllMessages();
            return Ok(messages);
        }

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

        [HttpPut("{messageId}", Name = "PUT - Entrypoint for update Message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateMessage(int messageId, [FromBody] Message updatedMessage)
        {
            try
            {
                var message = _messageService.UpdateMessage(updatedMessage);

                return Ok(message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

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
