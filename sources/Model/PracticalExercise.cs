using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PracticalExercise
    {
        public int Id { get; set; }
        public Exercise Exercise { get; set; }
        public Session Session { get; set; }
        public List<Set>? Sets { get; set; } = null;
    }
}
