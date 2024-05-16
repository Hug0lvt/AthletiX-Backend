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
        public string Username { get; set; } = string.Empty;
        public string UniqueNotificationToken { get; set; } = string.Empty;
        public Role Role { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public float Weight { get; set; }
        public float Height { get; set; }
        public bool Gender { get; set; }
        public string Picture { get; set; } = string.Empty; // en base64 en base de données

    }
}
