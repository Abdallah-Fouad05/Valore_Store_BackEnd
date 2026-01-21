using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;

namespace DAL.DTos
{
    public class OrderDTO_Updated
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
    }
    public class OrderDTO_Created
    {
        public int UserID { get; set; }
    }
    public class orderDTO_Details
    {
        public int OrderID { get; set; }
        public UserDTO User { get; set; }
        public DateTime CreatedAt { get; set; }
        public float TotalAmount { get; set; }

        public orderDTO_Details(int orderid,UserDTO user,DateTime createdat,float totalamount)
        { 
            OrderID = orderid;
            User = user;
            CreatedAt = createdat;
            TotalAmount = totalamount;
        }

    }
    public class orderDTO
    {
        public int OrderID { get; set; }
        public UserDTO User { get; set; }
        public DateTime CreatedAt { get; set; }
        public float TotalAmount { get; set; }

        public orderDTO(int orderid, UserDTO user, DateTime createdat, float totalamount)
        {
            OrderID = orderid;
            User = user;
            CreatedAt = createdat;
            TotalAmount = totalamount;
        }

    }
}
