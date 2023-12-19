using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Session
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
