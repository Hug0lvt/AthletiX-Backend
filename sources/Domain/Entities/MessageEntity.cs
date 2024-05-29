using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }

        public int ConversationId { get; set; }
        [ForeignKey(nameof(ConversationId))]
        public virtual ConversationEntity Conversation { get; set; }

        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public ProfileEntity Sender { get; set; }

        [Required]
        [MaxLength(3000)]
        public string Content { get; set; } = string.Empty;
        [Required]
        public DateTime DateSent { get; set; }

    }
}
