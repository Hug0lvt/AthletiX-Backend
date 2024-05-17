using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public DateTime PublishDate { get; set; }
        public ProfileEntity Publisher { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
