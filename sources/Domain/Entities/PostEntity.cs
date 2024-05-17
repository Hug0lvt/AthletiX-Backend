using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dommain.Entities
{
    public class PostEntity
    {
        public int Id { get; set; }
        public ProfileEntity Publisher { get; set; }
        public CategoryEntity Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PublicationType PublicationType { get; set; }
        public string Content { get; set; } = string.Empty;

    }
}
