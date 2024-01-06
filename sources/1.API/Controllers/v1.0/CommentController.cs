using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Comment APIs")]
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            var createdComment = _commentService.CreateComment(comment);
            return CreatedAtAction(nameof(GetCommentById), new { commentId = createdComment.Id }, createdComment);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllComments()
        {
            var comments = _commentService.GetAllComments();
            return Ok(comments);
        }

        [HttpGet("{commentId}", Name = "GET - Entrypoint for get Comment by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCommentById(int commentId)
        {
            var comment = _commentService.GetCommentById(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPut("{commentId}", Name = "PUT - Entrypoint for update Comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateComment(int commentId, [FromBody] Comment updatedComment)
        {
            try
            {
                var comment = _commentService.UpdateComment(updatedComment);

                return Ok(comment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{commentId}", Name = "DELETE - Entrypoint for remove Comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                var deletedComment = _commentService.DeleteComment(commentId);

                return Ok(deletedComment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
