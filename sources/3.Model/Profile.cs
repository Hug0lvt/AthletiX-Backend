using Shared.Enums;
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
        public int Id { get; set; }
        public string Username { get; set; }
        public string Mail {  get; set; }
        public string UniqueNotificationToken { get; set; }
        public Role Role { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }

    }
}
