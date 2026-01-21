using DAL;
using DAL.DTos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public class CartBusiness
    {

        // ================== Get All Carts ==================
        public static async Task<List<CartDTO>> GetAllCarts()
        {
              return await clsCartData.GetAllCarts();
           
        }

        // ================== Get Cart By ID ==================
        public static async Task<CartDTO?> GetCartById(int cartId)
        {
            try
            {
                if (cartId <= 0)
                    throw new ArgumentException("CartID must be greater than 0.");

                return await clsCartData.GetCartById(cartId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching the cart with ID {cartId}.", ex);
            }
        }

        // ================== Create Cart ==================
        public static  (bool success, int cartId) CreateCart(CartDTO_Created newCart)
        {
            try
            {
                if (newCart.UserID <= 0)
                    throw new ArgumentException("UserID must be greater than 0.");

                return clsCartData.CreateCart(newCart);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the cart.", ex);
            }
        }

        // ================== Update Cart ==================
        public static bool UpdateCart(CartDTO cart)
        {
            try
            {
                if (cart.CartID <= 0)
                    throw new ArgumentException("CartID must be greater than 0.");
                if (cart.UserID <= 0)
                    throw new ArgumentException("UserID must be greater than 0.");

                return clsCartData.UpdateCart(cart);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the cart with ID {cart.CartID}.", ex);
            }
        }

        // ================== Delete Cart ==================
        public static bool DeleteCart(int cartId)
        {
            try
            {
                if (cartId <= 0)
                    throw new ArgumentException("CartID must be greater than 0.");

                return clsCartData.DeleteCart(cartId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the cart with ID {cartId}.", ex);
            }
        }

        //================ Get Cart Item ===================
        public static List<CartItemDetails> GetCartItems(int userID)
        {
            try
            {
                return clsCartData.GetCartItems(userID); // Assuming your DAL class name
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching cart items for user ID {userID}.", ex);
            }
        }
    }
}
