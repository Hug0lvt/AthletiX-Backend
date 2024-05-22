using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommain.Entities
{
    public class ProfileEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        public string UniqueNotificationToken { get; set; } = string.Empty;
        public Role Role { get; set; }
        public int Age { get; set; }

        [Required]
        [MaxLength(30)]
        public string Email { get; set; } = string.Empty;

        public float Weight { get; set; }
        public float Height { get; set; }
        public bool Gender { get; set; }
        public string Picture { get; set; } = string.Empty; // en base64 en base de données

    }
}
