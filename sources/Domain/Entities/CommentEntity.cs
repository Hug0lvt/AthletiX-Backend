using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        public int ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public virtual ProfileEntity Publisher { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual PostEntity Post { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public int? ParentCommentId { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public virtual CommentEntity ParentComment { get; set; }
    }
}
