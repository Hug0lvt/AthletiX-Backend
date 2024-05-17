using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class ConversationMapper : AutoMapper.Profile
    {
        public ConversationMapper()
        {
            CreateMap<Conversation, ConversationEntity>();
            CreateMap<ConversationEntity, Conversation>();
        }
    }
}
