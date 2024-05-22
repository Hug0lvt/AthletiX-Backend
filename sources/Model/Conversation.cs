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
        public string Name { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty; // image en base64
        public List<Profile> Profiles { get; set; } = new List<Profile>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
