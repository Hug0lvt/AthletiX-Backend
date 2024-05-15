using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to posts.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Post APIs")]
    [ApiController]
    [Route("api/posts")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostController"/> class.
        /// </summary>
        /// <param name="postService">The post service.</param>
        public PostController(PostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post to create.</param>
        /// <returns>The newly created post.</returns>
        [HttpPost(Name = "POST - Entrypoint for create Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreatePost([FromBody] Post post)
        {
            var createdPost = _postService.CreatePost(post);
            return CreatedAtAction(nameof(GetPostById), new { postId = createdPost.Id }, createdPost);
        }

        /// <summary>
        /// Gets a post by its identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>The post with the specified identifier.</returns>
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

        /// <summary>
        /// Gets a post by profile identifier.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <returns>The posts with the specified identifier.</returns>
        [HttpGet("user/{profileId}", Name = "GET - Entrypoint for get Post by User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostByUser(int profileId)
        {
            var posts = _postService.GetPostByProfileId(profileId);

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        /// <summary>
        /// Retrieves a paginated list of posts by category identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <param name="index">The page index (0-based).</param>
        /// <param name="number">The number of posts per page.</param>
        /// <returns>An IActionResult containing the paginated list of posts with the specified category id.</returns>
        [HttpGet("category/{categoryId}", Name = "GET - Entrypoint for retrieving posts by category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostByCategory(int categoryId, int index, int number)
        {
            var post = _postService.GetPostsByCategoryId(categoryId, index, number);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        /// <summary>
        /// Updates a post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <param name="updatedPost">The updated post information.</param>
        /// <returns>The updated post.</returns>
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

        /// <summary>
        /// Deletes a post by its identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>The deleted post.</returns>
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
