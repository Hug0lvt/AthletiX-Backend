using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DataUser
    {
        public List<Conversation> Conversations { get; set; } = new List<Conversation>();
        public List<Post> Posts { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
