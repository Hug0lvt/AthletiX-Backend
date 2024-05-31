using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Session
    {
        public int Id { get; set; }
        public Profile Profile { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public List<PracticalExercise> Exercises { get; set; } = new List<PracticalExercise>();

    }
}
