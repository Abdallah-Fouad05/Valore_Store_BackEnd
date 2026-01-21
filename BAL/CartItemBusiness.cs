using DAL;
using DAL.DTos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public class CartItemBusiness
    {
        public static async Task<List<CartItemDTO>> GetAllCartItems()
        {
            try
            {
                return await CartItemData.GetAllCartItems();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all cart items.", ex);
            }
        }

        public static async Task<List<CartItemDTO>?> GetCartItemByID(int cartItemID)
        {
            try
            {
                return await CartItemData.GetCartItemsByID(cartItemID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching cart item with ID {cartItemID}.", ex);
            }
        }

        public static (bool success, int cartItemID) CreateCartItem(CartItem_Created cart)
        {
            try
            {
                
                return CartItemData.CreateCartItem(cart);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating cart item.", ex);
            }
        }

        public static bool UpdateCartItem(CartItem_Updated cart)
        {
            try
            {
                return CartItemData.UpdateCartItem(cart);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while updating cart item with ID {cart.CartItemID}.", ex);
            }
        }

        public static bool DeleteCartItem(int cartItemID)
        {
            try
            {
                return CartItemData.DeleteCartItem(cartItemID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while deleting cart item with ID {cartItemID}.", ex);
            }
        }
    }
}
