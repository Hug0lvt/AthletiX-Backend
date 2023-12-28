﻿using Shared.Enums;
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
        public Role Role { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public DataUser Data { get; set; }

        public Profile(int id, string username, Role role, int age, string email, float weight, float height, DataUser data)
        {
            Id = id;
            Username = username;
            Role = role;
            Age = age;
            Email = email;
            Weight = weight;
            Height = height;
            Data = data;
        }
    }
}
