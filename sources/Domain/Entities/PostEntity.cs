using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dommain.Entities
{
    public class PostEntity
    {
        [Key]
        public int Id { get; set; }

        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual ProfileEntity Publisher { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual CategoryEntity Category { get; set; }
        
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PublicationType PublicationType { get; set; }
        public string Content { get; set; } = string.Empty;

    }
}
