using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Category Category { get; set; }
        public Session Session { get; set; }
        public List<Set> Sets { get; set; } = new List<Set>();
    }
}
