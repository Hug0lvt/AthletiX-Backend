using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model
{
    public class Post
    {
        public int Id { get; set; }
        public Profile Publisher { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PublicationType PublicationType { get; set; }
        public string Content { get; set; } = string.Empty;

    }
}
