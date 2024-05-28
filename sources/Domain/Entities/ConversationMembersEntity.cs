using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class ConversationMembersEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ConversationId { get; set; }
        [ForeignKey(nameof(ConversationId))]
        public virtual ConversationEntity Conversation { get; set; }

        [Required]
        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual ProfileEntity Profile { get; set; }

    }
}
