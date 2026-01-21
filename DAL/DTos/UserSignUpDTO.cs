using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class UserSignUpDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
    public class CreatedUser
    {
        public string Email { get; set; }
        public int UserID { get; set; }
    }
}
