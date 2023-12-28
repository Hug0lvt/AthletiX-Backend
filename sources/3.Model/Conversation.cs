using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Conversation
    {
        public int Id { get; set; }
        public List<Profile> Profiles { get; set; } = new List<Profile>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
