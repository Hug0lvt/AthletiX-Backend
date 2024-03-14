using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to comments.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Comment APIs")]
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentController"/> class.
        /// </summary>
        /// <param name="commentService">The comment service.</param>
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The comment to create.</param>
        /// <returns>The newly created comment.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            var createdComment = _commentService.CreateComment(comment);
            return CreatedAtAction(nameof(GetCommentById), new { commentId = createdComment.Id }, createdComment);
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>A list of all comments.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllComments()
        {
            var comments = _commentService.GetAllComments();
            return Ok(comments);
        }

        /// <summary>
        /// Gets a comment by its identifier.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns>The comment with the specified identifier.</returns>
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

        /// <summary>
        /// Updates a comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <param name="updatedComment">The updated comment information.</param>
        /// <returns>The updated comment.</returns>
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

        /// <summary>
        /// Deletes a comment by its identifier.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns>The deleted comment.</returns>
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
