using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Application.LevelUp.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }    
        public string Passwordhash { get; set; }
        public string Email { get; set; }
        public int IsAdmin { get; set; }
        public string Purpose { get; set; }
    }
}
