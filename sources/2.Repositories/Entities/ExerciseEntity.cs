using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ExerciseEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<int> SetsIds { get; set; } = new List<int>();

        public ExerciseEntity(int id, string name, string description, string image, int category, List<int> sets)
        {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            CategoryId = category;
            SetsIds = sets;
        }
    }
}
