using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class CommentMapper : AutoMapper.Profile
    {
        public CommentMapper()
        {
            CreateMap<Comment, CommentEntity>();
            CreateMap<CommentEntity, Comment>();
        }
    }
}
