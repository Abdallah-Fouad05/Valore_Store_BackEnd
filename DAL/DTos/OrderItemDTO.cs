using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class OrderItemDTO_Datelis
    {
        public int OrderItemID { get; set; }
        public orderDTO Order { get; set; }
        public ProductDTO Product { get; set; }
        public StatusDTO Status { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }
        public string Address { get; set; }

        public OrderItemDTO_Datelis(int orderitemid,orderDTO order,ProductDTO product, StatusDTO status, int quantity, float price,string address)
        {
            OrderItemID = orderitemid;
            Order = order;
            Product = product;
            Status = status;
            Quantity = quantity;
            Price = price;
            TotalPrice = (price * quantity);
            Address = address;
        }

    }
    
    public class OrderItemDTO_Created
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int  StatusID { get; set; }  
        public int Quantity { get; set; }
        public string Address{ get; set; }

        public OrderItemDTO_Created(int orderid,int productid,int statusid,int quantity,string address)
        {
            OrderID = orderid;
            ProductID = productid;
            StatusID = statusid;
            Quantity = quantity;
            Address = address;
        }


    }

    public class OrderItemDTO_Updated
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int StatusID { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }

        public OrderItemDTO_Updated(int orderitemid,int orderid, int productid,int statusid, int quantity, string address)
        {
            OrderItemID = orderitemid;
            OrderID = orderid;
            ProductID = productid;
            StatusID = statusid;
            Quantity = quantity;
            Address = address;
        }


    }
}
