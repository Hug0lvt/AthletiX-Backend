using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class SessionEntity
    {
        public int Id { get; set; }
        public ProfileEntity Profile { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
