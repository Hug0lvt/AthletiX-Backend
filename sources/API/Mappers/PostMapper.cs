using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class PostMapper : AutoMapper.Profile
    {
        public PostMapper()
        {
            CreateMap<Post, PostEntity>()
                .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.Publisher.Id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));

            CreateMap<PostEntity, Post>()
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => new Profile { Id = src.ProfileId }))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Id = src.CategoryId }));
        }
    }
}
