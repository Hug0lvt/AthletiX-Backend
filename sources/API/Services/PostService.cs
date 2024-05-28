using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using Shared;
using AutoMapper;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Domain.Entities;
using Profile = Model.Profile;

namespace API.Services
{
    /// <summary>
    /// Service for managing posts.
    /// </summary>
    public class PostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public PostService(IdentityAppDbContext dbContext, ILogger<PostService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post to be created.</param>
        /// <returns>The created post.</returns>
        public async Task<Post> CreatePostAsync(Post post)
        {
            try
            {
                var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == post.Category.Id);
                if (existingCategory == null)
                    throw new NotCreatedExecption("Category does not exist.");

                var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == post.Publisher.Id);
                if (existingProfile == null)
                    throw new NotCreatedExecption("Publisher does not exist.");

                var entity = _mapper.Map<PostEntity>(post);

                entity.CategoryId = existingCategory.Id;
                entity.ProfileId = existingProfile.Id;
                entity.Publisher = null;
                entity.Category = null;

                _dbContext.Entry(existingCategory).State = EntityState.Unchanged;
                _dbContext.Entry(existingProfile).State = EntityState.Unchanged;

                _dbContext.Posts.Add(entity);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<Post>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create post.", ex);
            }
        }


        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        public List<Post> GetAllPosts()
        {
            return _mapper.Map<List<Post>>(_dbContext.Posts.ToList());
        }

        /// <summary>
        /// Gets a post by its identifier.
        /// </summary>
        /// <param name="postId">The identifier of the post.</param>
        /// <returns>The post with the specified identifier.</returns>
        public Post GetPostById(int postId)
        {
            Post post = _mapper.Map<Post>(_dbContext.Posts
                .Include(p => p.Category)
                .Include(p => p.Publisher)
                .FirstOrDefault(p => p.Id == postId));

            if(post != null)
            {
                List<Comment> comments = _mapper.Map<List<Comment>>(_dbContext.Comments
                .Where(c => c.PostId == post.Id).ToList());
                post.Comments = comments;
            }

            return post;
        }

        /// <summary>
        /// Gets posts by category identifier with pagination.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <returns>The paginated list of posts with the specified category id.</returns>
        public PaginationResult<Post> GetPostsByCategory(int categoryId, bool includeComments = false)
        {
            var items = _mapper.Map<List<Post>>(_dbContext.Posts.Include(p => p.Category).Include(p => p.Publisher)
                .Where(p => p.Category.Id == categoryId)
                .ToList());

            if (includeComments)
            {
                foreach (var post in items)
                {
                    if (post != null)
                    {
                        List<Comment> comments = _mapper.Map<List<Comment>>(_dbContext.Comments
                        .Where(c => c.PostId == post.Id).ToList());
                        post.Comments = comments;
                    }
                }
            }

            var totalItems = items.Count();

            return new PaginationResult<Post>
            {
                Items = items,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets posts by user identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the user.</param>
        /// <returns>The list of posts with the specified user id.</returns>
        public PaginationResult<Post> GetPostsByUser(int userId, bool includeComments = false)
        {
            var items = _mapper.Map<List<Post>>(_dbContext.Posts.Include(p => p.Category).Include(p => p.Publisher)
                .Where(p => p.Publisher.Id == userId)
                .ToList());

            if (includeComments)
            {
                foreach (var post in items)
                {
                    if (post != null)
                    {
                        List<Comment> comments = _mapper.Map<List<Comment>>(_dbContext.Comments
                        .Where(c => c.PostId == post.Id).ToList());
                        post.Comments = comments;
                    }
                }
            }

            var totalItems = items.Count();

            return new PaginationResult<Post>
            {
                Items = items,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="updatedPost">The updated post.</param>
        /// <returns>The updated post.</returns>
        public Post UpdatePost(int postId, Post updatedPost)
        {
            if (postId != updatedPost.Id)
                updatedPost.Id = postId;

            var existingPost = _dbContext.Posts.Include(p => p.Category).Include(p => p.Publisher).FirstOrDefault(p => p.Id == updatedPost.Id);

            if (existingPost != null)
            {
                existingPost.Title = updatedPost.Title;
                existingPost.Content = updatedPost.Content;
                existingPost.Description = updatedPost.Description;
                existingPost.CategoryId = updatedPost.Category.Id;
                existingPost.ProfileId = updatedPost.Publisher.Id;
                existingPost.PublicationType = updatedPost.PublicationType;

                _dbContext.SaveChanges();

                return _mapper.Map<Post>(existingPost);
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
                return _mapper.Map<Post>(postToDelete);
            }

            _logger.LogTrace("[LOG | PostService] - (DeletePost): Post not found");
            throw new NotFoundException("[LOG | PostService] - (DeletePost): Post not found");
        }

        /// <summary>
        /// Add a like on post.
        /// </summary>
        public async Task<bool> LikePost(int postId, int profileId)
        {
            var existingPost = await _dbContext.Posts.FirstOrDefaultAsync(c => c.Id == postId);
            if (existingPost == null)
                throw new NotCreatedExecption("Post does not exist.");

            var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            if (existingProfile == null)
                throw new NotCreatedExecption("Profile does not exist.");

            _dbContext.LikedPosts.Add(new LikedPostEntity { Id = 0, LikedPostId = postId, LikedByThisProfileId = profileId });
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Remove a like on a post.
        /// </summary>
        public async Task<bool> UnlikePost(int postId, int profileId)
        {
            var existingPost = await _dbContext.Posts.FirstOrDefaultAsync(c => c.Id == postId);
            if (existingPost == null)
                throw new NotCreatedExecption("Post does not exist.");

            var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            if (existingProfile == null)
                throw new NotCreatedExecption("Profile does not exist.");

            var existingPostLikedByThisProfile = await _dbContext.LikedPosts
                .FirstOrDefaultAsync(l => l.LikedPostId == postId && l.LikedByThisProfileId == profileId);
            if (existingPostLikedByThisProfile == null)
                throw new NotFoundException("Profile not like this post.");

            _dbContext.LikedPosts.Remove(existingPostLikedByThisProfile);
            _dbContext.SaveChanges();
            return true;
        }

        public PaginationResult<Profile> GetLikesPostById(int postId)
        {
            List<Profile> profiles = _mapper.Map<List<Profile>>(_dbContext.LikedPosts
                .Include(p => p.LikedByThisProfile)
                .Where(p => p.LikedPostId == postId)
                .Select(p => p.LikedByThisProfile));

            return new PaginationResult<Profile>
            {
                Items = profiles,
                TotalItems = profiles.Count()
            };
        }

        public PaginationResult<Post> GetPostLikedByProfile(int profileId)
        {
            List<Post> posts = _mapper.Map<List<Post>>(_dbContext.LikedPosts
                .Include(p => p.LikedPost)
                .Where(p => p.LikedByThisProfileId == profileId)
                .Select(p => p.LikedPost));

            return new PaginationResult<Post>
            {
                Items = posts,
                TotalItems = posts.Count()
            };
        }
    }
}
