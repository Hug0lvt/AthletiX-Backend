using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class SetEntity
    {
        public int Id { get; set; }
        public int Reps { get; set; }
        public List<float> Weight { get; set; } = new List<float>();
        public TimeSpan Rest { get; set; }
        public SetMode Mode { get; set; }

        public SetEntity(int id, int reps, List<float> weight, TimeSpan rest, SetMode mode)
        {
            Id = id;
            Reps = reps;
            Weight = weight;
            Rest = rest;
            Mode = mode;
        }
    }
}
