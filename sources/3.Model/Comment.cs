using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Comment
    {
        public int Id { get; set; }
        public Profile Publisher { get; set; }
        public string Content { get; set; }
        public List<Comment> Answers { get; set; } = new List<Comment>();
    }
}
