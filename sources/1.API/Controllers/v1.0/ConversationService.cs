using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Conversation APIs")]
    [ApiController]
    [Route("api/conversations")]
    public class ConversationController : ControllerBase
    {
        private readonly ConversationService _conversationService;

        public ConversationController(ConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateConversation([FromBody] Conversation conversation)
        {
            var createdConversation = _conversationService.CreateConversation(conversation);
            return CreatedAtAction(nameof(GetConversationById), new { conversationId = createdConversation.Id }, createdConversation);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Conversation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllConversations()
        {
            var conversations = _conversationService.GetAllConversations();
            return Ok(conversations);
        }

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

        [HttpDelete("{conversationId}", Name = "PUT - Entrypoint for update Conversation")]
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
    }
}
