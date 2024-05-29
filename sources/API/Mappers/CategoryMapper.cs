using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class CategoryMapper : AutoMapper.Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryEntity>();
            CreateMap<CategoryEntity, Category>();
        }
    }
}
