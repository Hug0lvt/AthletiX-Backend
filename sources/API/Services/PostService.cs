using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;
using Shared.Mappers;

namespace API.Services
{
    /// <summary>
    /// Service for managing posts.
    /// </summary>
    public class PostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public PostService(IdentityAppDbContext dbContext, ILogger<PostService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post to be created.</param>
        /// <returns>The created post.</returns>
        public Post CreatePost(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return post;
        }

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        public List<Post> GetAllPosts()
        {
            return _dbContext.Posts.ToList();
        }

        /// <summary>
        /// Gets a post by its identifier.
        /// </summary>
        /// <param name="postId">The identifier of the post.</param>
        /// <returns>The post with the specified identifier.</returns>
        public Post GetPostById(int postId)
        {
            return _dbContext.Posts.FirstOrDefault(p => p.Id == postId);
        }

        /// <summary>
        /// Gets posts by category identifier with pagination.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <returns>The paginated list of posts with the specified category id.</returns>
        public PaginationResult<Post> GetPostsByCategory(int categoryId)
        {
            var items = _dbContext.Posts
                .Where(p => p.Category.Id == categoryId)
                .ToList();
            var totalItems = items.Count();

            return new PaginationResult<Post>
            {
                Items = items,
                NextPage = -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets posts by user identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the user.</param>
        /// <returns>The list of posts with the specified user id.</returns>
        public PaginationResult<Post> GetPostsByUser(int userId)
        {
            var items = _dbContext.Posts
                .Where(p => p.Publisher.Id == userId)
                .ToList();
            var totalItems = items.Count();

            return new PaginationResult<Post>
            {
                Items = items,
                NextPage = -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="updatedPost">The updated post.</param>
        /// <returns>The updated post.</returns>
        public Post UpdatePost(Post updatedPost)
        {
            var existingPost = _dbContext.Posts.Find(updatedPost.Id);

            if (existingPost != null)
            {
                existingPost.Title = updatedPost.Title;
                existingPost.Description = updatedPost.Description;
                // You may want to include updating the content if necessary
                _dbContext.SaveChanges();
                return existingPost;
            }

            _logger.LogTrace("[LOG | PostService] - (UpdatePost): Post not found");
            throw new NotFoundException("[LOG | PostService] - (UpdatePost): Post not found");
        }

        /// <summary>
        /// Deletes a post by its identifier.
        /// </summary>
        /// <param name="postId">The identifier of the post to be deleted.</param>
        /// <returns>The deleted post.</returns>
        public Post DeletePost(int postId)
        {
            var postToDelete = _dbContext.Posts.Find(postId);

            if (postToDelete != null)
            {
                _dbContext.Posts.Remove(postToDelete);
                _dbContext.SaveChanges();
                return postToDelete;
            }

            _logger.LogTrace("[LOG | PostService] - (DeletePost): Post not found");
            throw new NotFoundException("[LOG | PostService] - (DeletePost): Post not found");
        }
    }
}
