using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DateSent { get; set; }
        public ProfileEntity Sender { get; set; }

    }
}
