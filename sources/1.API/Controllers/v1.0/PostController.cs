using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Post APIs")]
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpPost(Name = "POST - Entrypoint for create Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreatePost([FromBody] Post post)
        {
            var createdPost = _postService.CreatePost(post);
            return CreatedAtAction(nameof(GetPostById), new { postId = createdPost.Id }, createdPost);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("{postId}", Name = "GET - Entrypoint for get Post by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostById(int postId)
        {
            var post = _postService.GetPostById(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPut("{postId}", Name = "PUT - Entrypoint for update Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePost(int postId, [FromBody] Post updatedPost)
        {
            try
            {
                var post = _postService.UpdatePost(updatedPost);

                return Ok(post);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{postId}", Name = "DELETE - Entrypoint for remove Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                var deletedPost = _postService.DeletePost(postId);

                return Ok(deletedPost);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
