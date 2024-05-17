using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class MessageMapper : AutoMapper.Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, MessageEntity>();
            CreateMap<MessageEntity, Message>();
        }
    }
}
