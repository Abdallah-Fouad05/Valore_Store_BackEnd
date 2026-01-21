using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class UserLogInDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class UserLogInResult
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
