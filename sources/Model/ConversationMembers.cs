using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ConversationMembers
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int ProfileId { get; set; }
    }
}
