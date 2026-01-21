using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class CartDTO
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CartDTO(int cartid, int userid, DateTime createdAt, DateTime updatedat)
        {
            CartID = cartid;
            UserID = userid;
            CreatedAt = createdAt;
            UpdatedAt = updatedat;
        }
    }

    public class CartDTO_Created
    {
        public int UserID { get; set; }
        public CartDTO_Created(int userid) {
            UserID = userid;
        }
    }
}