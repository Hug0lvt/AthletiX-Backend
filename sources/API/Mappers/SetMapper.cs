using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class SetMapper : AutoMapper.Profile
    {
        public SetMapper()
        {
            CreateMap<Set, SetEntity>();
            CreateMap<SetEntity, Set>();
        }
    }
}
