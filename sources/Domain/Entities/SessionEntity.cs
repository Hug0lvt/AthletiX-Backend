using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class SessionEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual ProfileEntity Profile { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
