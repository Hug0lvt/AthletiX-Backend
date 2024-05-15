using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;
using Shared.Mappers;

namespace API.Services
{
    /// <summary>
    /// Service for managing comments.
    /// </summary>
    public class CommentService
    {
        private readonly ILogger<CommentService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CommentService(IdentityAppDbContext dbContext, ILogger<CommentService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The comment to be created.</param>
        /// <returns>The created comment.</returns>
        public Comment CreateComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>A list of all comments.</returns>
        public List<Comment> GetAllComments()
        {
            return _dbContext.Comments.ToList();
        }

        /// <summary>
        /// Gets all comments (with pages).
        /// </summary>
        /// <returns>A list of all comments.</returns>
        public PaginationResult<Comment> GetAllCommentsWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Comments.Count();
            var items = _dbContext.Comments
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<Comment>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
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
            return _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);
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
                return existingComment;
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
                return commentToDelete;
            }

            _logger.LogTrace("[LOG | CommentService] - (DeleteComment): Comment not found");
            throw new NotFoundException("[LOG | CommentService] - (DeleteComment): Comment not found");
        }
    }
}
