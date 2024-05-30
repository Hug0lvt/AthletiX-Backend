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
    public class PracticalExerciseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public virtual ExerciseEntity Exercise { get; set; }

        [Required]
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public virtual SessionEntity Session { get; set; }
    }
}
