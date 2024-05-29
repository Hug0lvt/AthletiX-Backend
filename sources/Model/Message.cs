using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
        public Profile Sender { get; set; }
        public int ConversationId { get; set; }

    }
}
