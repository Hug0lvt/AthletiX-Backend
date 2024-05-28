using Dommain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LikedPostEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LikedByThisProfileId { get; set; }
        [ForeignKey(nameof(LikedByThisProfileId))]
        public virtual ProfileEntity LikedByThisProfile { get; set; }

        [Required]
        public int LikedPostId { get; set; }
        [ForeignKey(nameof(LikedPostId))]
        public virtual PostEntity LikedPost { get; set; }
    }
}
