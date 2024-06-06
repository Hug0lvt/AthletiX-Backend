using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dommain.Entities;

namespace Domain.Entities;

public class ExerciseInPostEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int ExerciseId { get; set; }
    [ForeignKey(nameof(ExerciseId))]
    public virtual ExerciseEntity Exercise { get; set; }

    [Required]
    public int PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public virtual PostEntity Post { get; set; }
}