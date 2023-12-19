using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Conversation
    {
        public Dictionary<List<Profile>, List<Message>> Messages { get; set; } = new Dictionary<List<Profile>, List<Message>>();
    }
}
