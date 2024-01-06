using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class CommentService
    {
        private readonly AppDbContext _dbContext;

        public CommentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Comment CreateComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public List<Comment> GetAllComments()
        {
            return _dbContext.Comments.ToList();
        }

        public Comment GetCommentById(int commentId)
        {
            return _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);
        }

        public Comment UpdateComment(Comment updatedComment)
        {
            var existingComment = _dbContext.Comments.Find(updatedComment.Id);

            if (existingComment != null)
            {
                existingComment.Content = updatedComment.Content;
                _dbContext.SaveChanges();
                return existingComment;
            }

            throw new NotFoundException("[LOG | CommentService] - (UpdateComment): Comment not found");
        }

        public Comment DeleteComment(int commentId)
        {
            var commentToDelete = _dbContext.Comments.Find(commentId);

            if (commentToDelete != null)
            {
                _dbContext.Comments.Remove(commentToDelete);
                _dbContext.SaveChanges();
                return commentToDelete;
            }

            throw new NotFoundException("[LOG | CommentService] - (DeleteComment): Comment not found");
        }
    }
}
