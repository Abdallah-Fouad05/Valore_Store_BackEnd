using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class CartItemDTO
    {
        public int CartItemID { get; set; }
        public CartDTO Cart {  get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CartItemDTO(int cartItemid,CartDTO cart,ProductDTO product, int quantity,DateTime createdat,DateTime updatedat) 
        {
            CartItemID = cartItemid;
            Cart = cart;
            Product = product;
            Quantity = quantity;
            CreatedAt = createdat;
            UpdatedAt = updatedat;
        }
    }
    public class CartItem_Created
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }

        public CartItem_Created(int productID,int userid,int quantity)
        {
            ProductID= productID;
            UserID= userid;
            Quantity = quantity;
        }

    }
    public class CartItem_Updated
    {
        public int CartItemID { get; set; }
        public int Quantity { get; set; }

        public CartItem_Updated(int cartItemid, int quantity)
        {
            CartItemID = cartItemid;
            Quantity = quantity;
        }

    }

    public class CartItemDetails
    {
        public int CartItemID {get; set;}
        public int ProductID { get; set; }
        public string ProductName { get; set; }      
        public int Quantity { get; set;}
        public float Price { get; set; }
        public float TotalPrice { get; set; }   
        public int CartID {get; set;}
        public string ImageURL { get; set;}

        public CartItemDetails(int cartItemID, int productID, string productName, int quantity, float price, float totalPrice, int cartID, string imageURL)
        {
            CartItemID = cartItemID;
            ProductID = productID;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
            CartID = cartID;
            ImageURL = imageURL;
        }
    }
}
