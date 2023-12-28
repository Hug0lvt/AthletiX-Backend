using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ConversationEntity
    {
        public int Id { get; set; }
        public List<int> ProfilesIds { get; set; } = new List<int>();
        public List<int> MessagesIds { get; set; } = new List<int>();

        public ConversationEntity(int id, List<int> profilesIds, List<int> messagesIds)
        {
            Id = id;
            ProfilesIds = profilesIds;
            MessagesIds = messagesIds;
        }
    }
}
