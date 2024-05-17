using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class PostMapper : AutoMapper.Profile
    {
        public PostMapper()
        {
            CreateMap<Post, PostEntity>();
            CreateMap<PostEntity, Post>();
        }
    }
}
