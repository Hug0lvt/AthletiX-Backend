using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories.Entities
{
    public class ProfileEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public int DataId { get; set; }

        public ProfileEntity(int id, string username, Role role, int age, string email, float weight, float height, int data)
        {
            Id = id;
            Username = username;
            Role = role;
            Age = age;
            Email = email;
            Weight = weight;
            Height = height;
            DataId = data;
        }
    }
}
