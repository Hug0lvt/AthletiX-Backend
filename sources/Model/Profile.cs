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
        public string UniqueNotificationToken { get; set; }
        public Role Role { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public bool Gender { get; set; }
        public string Picture { get; set; } = string.Empty; // en base64 en base de données

        public override string ToString()
        {
            string roleName = Role != null ? Role.ToString() : "No Role";
            string genderString = Gender ? "Male" : "Female";
        
            return $"Profile Id: {Id}, Username: {Username}, Notification Token: {UniqueNotificationToken}, Role: {roleName}, Age: {Age}, Email: {Email}, Weight: {Weight} kg, Height: {Height} m, Gender: {genderString}, Picture Length: {Picture.Length} characters";
        }
    }
}
