using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using Shared.Exceptions;
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
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "athv1", "posts");
        string videoBaseUri = Environment.GetEnvironmentVariable("VIDEO_BASE_URI", EnvironmentVariableTarget.Process) ?? "";

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
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            var createdPost = await _postService.CreatePostAsync(post);
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
        /// Retrieves a paginated list of posts by category identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <returns>An IActionResult containing the paginated list of posts with the specified category id.</returns>
        [HttpGet("category/{categoryId}", Name = "GET - Entrypoint for retrieving posts by category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostByCategory(int categoryId, bool includeComments = false)
        {
            var post = _postService.GetPostsByCategory(categoryId, includeComments);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        /// <summary>
        /// Retrieves a paginated list of posts by user identifier.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>An IActionResult containing the paginated list of posts with the specified user id.</returns>
        [HttpGet("user/{userId}", Name = "GET - Entrypoint for retrieving posts by user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostByUser(int userId, bool includeComments = false)
        {
            var post = _postService.GetPostsByUser(userId, includeComments);

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
                var post = _postService.UpdatePost(postId, updatedPost);
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

        /// <summary>
        /// Add a like to a post.
        /// </summary>
        [HttpPost("{postId}/likedby/{profileId}", Name = "POST - Entrypoint for like post by profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMemberInConversation(int postId, int profileId)
        {
            try
            {
                return Ok(await _postService.LikePost(postId, profileId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Unlike post with profile.
        /// </summary>
        [HttpDelete("{postId}/unlikedby/{profileId}", Name = "POST - Entrypoint for unlike post by profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveMemberInConversation(int postId, int profileId)
        {
            try
            {
                return Ok(await _postService.UnlikePost(postId, profileId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a number of likes on this post by its identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>The post with the specified identifier.</returns>
        [HttpGet("{postId}/likes", Name = "GET - Entrypoint for get likes Post by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLikesPostById(int postId)
        {
            return Ok(_postService.GetLikesPostById(postId));
        }

        /// <summary>
        /// Retrieves a paginated list of liked posts by user identifier.
        /// </summary>
        [HttpGet("likedby/{userId}", Name = "GET - Entrypoint for retrieving liked posts by user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLikedPostByUser(int userId)
        {
            return Ok(_postService.GetPostLikedByProfile(userId));
        }

        [HttpPost("{postId}/upload")]
        public async Task<IActionResult> Upload(IFormFile file, int postId)
        {
            if (file == null || file.Length == 0) return BadRequest("No file provided.");

            var existingPost = _postService.GetPostById(postId);
            if(existingPost == null) return BadRequest("Post Not Exists");

            var originalFileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(originalFileName);
            var newFileName = $"Ath-{originalFileName}-{DateTime.Now.ToString("yyyyMMddHHmmss")}-{postId}{extension}";
            var filePath = Path.Combine(_storagePath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{videoBaseUri}/videos/{newFileName}"; // TODO A test sur codefirst

            existingPost.Content = fileUrl;
            if (_postService.IsVideoExtension(extension)) existingPost.PublicationType = Shared.Enums.PublicationType.Video;
            if (_postService.IsImageExtension(extension)) existingPost.PublicationType = Shared.Enums.PublicationType.Image;

            return Ok(_postService.UpdatePost(postId, existingPost));
        }

        /// <summary>
        /// Retrieves a paginated list of posts recomended for user.
        /// </summary>
        [HttpGet("recommendations/user/{userId}", Name = "GET - Entrypoint for retrieving recommended posts by user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRecommendationsPostByUser(int userId, int pageSize = 10)
        {
            return Ok(_postService.GetRecommendedPosts(userId, pageSize));
        }
    }
}
