using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class ExerciseEntity
    {
        [Key]
        public int Id { get; set; }

        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public virtual SessionEntity Session { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual CategoryEntity Category { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
