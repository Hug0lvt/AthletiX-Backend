using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class SessionMapper : AutoMapper.Profile
    {
        public SessionMapper()
        {
            CreateMap<Session, SessionEntity>();
            CreateMap<SessionEntity, Session>();
        }
    }
}
