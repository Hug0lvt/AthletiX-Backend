using Model.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Profile
    {
        public string Username { get; set; }
        public Role Role { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public DataUser Data { get; set; }
    }
}
