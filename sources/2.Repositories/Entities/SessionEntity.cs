using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class SessionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public List<int> ExercisesIds { get; set; } = new List<int>();

        public SessionEntity(int id, string name, DateTime date, TimeSpan duration, List<int> exercises)
        {
            Id = id;
            Name = name;
            Date = date;
            Duration = duration;
            ExercisesIds = exercises;
        }
    }
}
