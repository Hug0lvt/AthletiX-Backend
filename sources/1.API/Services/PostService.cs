using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class PostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly AppDbContext _dbContext;

        public PostService(AppDbContext dbContext, ILogger<PostService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Post CreatePost(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return post;
        }

        public List<Post> GetAllPosts()
        {
            return _dbContext.Posts.ToList();
        }

        public Post GetPostById(int postId)
        {
            return _dbContext.Posts.FirstOrDefault(p => p.Id == postId);
        }

        public Post UpdatePost(Post updatedPost)
        {
            var existingPost = _dbContext.Posts.Find(updatedPost.Id);

            if (existingPost != null)
            {
                existingPost.Title = updatedPost.Title;
                existingPost.Description = updatedPost.Description;
                // On Update Content ???
                _dbContext.SaveChanges();
                return existingPost;
            }
            _logger.LogTrace("[LOG | PostService] - (UpdatePost): Post not found");
            throw new NotFoundException("[LOG | PostService] - (UpdatePost): Post not found");
        }

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
