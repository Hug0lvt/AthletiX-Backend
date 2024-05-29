using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Set
    {
        public int Id { get; set; }
        public int Reps { get; set; }
        public List<float> Weight { get; set; } = new List<float>();
        public TimeSpan Rest { get; set; }
        public SetMode Mode { get; set; }
        public Exercise Exercise { get; set; }

    }
}
