using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
        public int SenderId { get; set; }

        public MessageEntity(int id, string content, DateTime dateSent, int sender)
        {
            Id = id;
            Content = content;
            DateSent = dateSent;
            SenderId = sender;
        }
    }
}
