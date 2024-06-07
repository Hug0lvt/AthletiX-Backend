using Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class SetEntity
    {
        [Key]
        public int Id { get; set; }

        public int PracticalExerciseId { get; set; }
        [ForeignKey(nameof(PracticalExerciseId))]
        public virtual PracticalExerciseEntity PracticalExercise { get; set; }

        public int Reps { get; set; }
        
        [NotMapped]
        public List<float> Weight { get; set; } = new List<float>();

        public string WeightJson
        {
            get => JsonSerializer.Serialize(Weight);
            set => Weight = string.IsNullOrEmpty(value)
                ? new List<float>() : JsonSerializer.Deserialize<List<float>>(value);
        }

        public TimeSpan Rest { get; set; }
        public SetMode Mode { get; set; }
        public bool IsDone { get; set; }

    }
}
