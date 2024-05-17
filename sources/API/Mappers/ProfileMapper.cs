using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class ProfileMapper : AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<Profile, ProfileEntity>();
            CreateMap<ProfileEntity, Profile>();
        }
    }
}
