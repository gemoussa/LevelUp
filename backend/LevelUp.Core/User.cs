﻿namespace LevelUp.Core
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
       
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int IsAdmin { get; set; }
        public int PurposeId { get; set; }
    

    }
}
