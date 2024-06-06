using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using Shared;
using AutoMapper;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace API.Services
{
    /// <summary>
    /// Service for managing comments.
    /// </summary>
    public class CommentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CommentService(IdentityAppDbContext dbContext, ILogger<CommentService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The comment to be created.</param>
        /// <returns>The created comment.</returns>
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            Debug.WriteLine(comment.ToString());
            Console.WriteLine(comment.ToString());
            _logger.LogTrace("[LOG | CommentService] - (UpdateComment): " + comment.ToString());
            try
            {
                var existingParentComment = await _dbContext.Comments.FirstOrDefaultAsync(p => p.ParentCommentId == comment.Id);
                if (existingParentComment == null)
                    comment.ParentCommentId = null;

                var existingPost = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == comment.Post.Id);
                if (existingPost == null)
                    throw new NotCreatedExecption("Post does not exist.");

                var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == comment.Publisher.Id);
                if (existingProfile == null)
                    throw new NotCreatedExecption("Publisher does not exist.");

                var entity = _mapper.Map<CommentEntity>(comment);

                entity.PostId = existingPost.Id;
                entity.ProfileId = existingProfile.Id;
                if(existingParentComment != null)
                {
                    entity.ParentCommentId = existingParentComment.Id;
                    entity.ParentComment = null;
                    _dbContext.Entry(existingParentComment).State = EntityState.Unchanged;
                } else
                {
                    entity.ParentCommentId = null;
                    entity.ParentComment = null;
                }
                    
                entity.Publisher = null;
                entity.Post = null;
                

                _dbContext.Entry(existingPost).State = EntityState.Unchanged;
                _dbContext.Entry(existingProfile).State = EntityState.Unchanged;
                

                _dbContext.Comments.Add(entity);
                await _dbContext.SaveChangesAsync();
                
                return _mapper.Map<Comment>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create comment.", ex);
            }
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>A list of all comments.</returns>
        public List<Comment> GetAllComments()
        {
            return _mapper.Map<List<Comment>>(_dbContext.Comments.ToList());
        }

        /// <summary>
        /// Gets all comments (with pages).
        /// </summary>
        /// <returns>A list of all comments.</returns>
        public PaginationResult<Comment> GetAllCommentsWithPages(
            int pageSize = 10,
            int pageNumber = 0, 
            bool includeAnswers = false)
        {
            var totalItems = _dbContext.Comments.Count();
            var items = _mapper.Map<List<Comment>>(_dbContext.Comments
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList());

            if (includeAnswers)
            {
                foreach (var comment in items)
                {
                    if (comment != null)
                    {
                        List<Comment> answers = _mapper.Map<List<Comment>>(_dbContext.Comments
                        .Include(c => c.Publisher)
                        .Where(c => c.ParentCommentId == comment.Id)
                        .ToList());
                        comment.Answers = answers;
                    }
                }
            }

            return new PaginationResult<Comment>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets all comments for a post.
        /// </summary>
        /// <returns>A list of all comments for one post.</returns>
        public PaginationResult<Comment> GetAllCommentsOnPost(int postId, bool includeAnswers = false)
        {
            var items = _mapper.Map<List<Comment>>(_dbContext.Comments.Where(q => q.PostId == postId)
                .ToList());

            if (includeAnswers)
            {
                foreach (var comment in items)
                {
                    if (comment != null)
                    {
                        List<Comment> answers = _mapper.Map<List<Comment>>(_dbContext.Comments
                        .Include(c => c.Publisher)
                        .Where(c => c.ParentCommentId == comment.Id)
                        .ToList());
                        comment.Answers = answers;
                    }
                }
            }

            var totalItems = items.Count();

            return new PaginationResult<Comment>
            {
                Items = items,
                NextPage = -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets a comment by its identifier.
        /// </summary>
        /// <param name="commentId">The identifier of the comment.</param>
        /// <returns>The comment with the specified identifier.</returns>
        public Comment GetCommentById(int commentId)
        {
            Comment comment = _mapper.Map<Comment>(_dbContext.Comments
                .Include(c => c.Post)
                .Include(c => c.Publisher)
                .FirstOrDefault(c => c.Id == commentId));

            if (comment != null)
            {
                List<Comment> answers = _mapper.Map<List<Comment>>(_dbContext.Comments
                .Include(c => c.Publisher)
                .Where(c => c.ParentCommentId == commentId)
                .ToList());
                comment.Answers = answers;
            }

            return comment;
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="updatedComment">The updated comment information.</param>
        /// <returns>The updated comment.</returns>
        public Comment UpdateComment(Comment updatedComment)
        {
            var existingComment = _dbContext.Comments.Find(updatedComment.Id);

            if (existingComment != null)
            {
                existingComment.Content = updatedComment.Content;
                _dbContext.SaveChanges();
                return _mapper.Map<Comment>(existingComment);
            }

            _logger.LogTrace("[LOG | CommentService] - (UpdateComment): Comment not found");
            throw new NotFoundException("[LOG | CommentService] - (UpdateComment): Comment not found");
        }

        /// <summary>
        /// Deletes a comment by its identifier.
        /// </summary>
        /// <param name="commentId">The identifier of the comment to be deleted.</param>
        /// <returns>The deleted comment.</returns>
        public Comment DeleteComment(int commentId)
        {
            var commentToDelete = _dbContext.Comments.Find(commentId);

            if (commentToDelete != null)
            {
                _dbContext.Comments.Remove(commentToDelete);
                _dbContext.SaveChanges();
                return _mapper.Map<Comment>(commentToDelete);
            }

            _logger.LogTrace("[LOG | CommentService] - (DeleteComment): Comment not found");
            throw new NotFoundException("[LOG | CommentService] - (DeleteComment): Comment not found");
        }
    }
}
