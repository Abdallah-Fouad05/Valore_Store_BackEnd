using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTos;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class CartItemData
    {
        public static async Task<List<CartItemDTO>> GetAllCartItems()
        {
            List<CartItemDTO> items = new();

            await using SqlConnection conn = new SqlConnection(clsSettings.Connection);
            await using SqlCommand cmd = new SqlCommand("sp_GetAllCartItems", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await conn.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    items.Add(new CartItemDTO(
                        cartItemid: Convert.ToInt32(reader["CartItemID"]),
                        new CartDTO(cartid: Convert.ToInt32(reader["CartID"]), userid: Convert.ToInt32(reader["UserID"]), createdAt: Convert.ToDateTime(reader["CreatedAt"]), updatedat: Convert.ToDateTime(reader["UpdatedAt"])),
                        new ProductDTO(productID: Convert.ToInt32(reader["ProductID"]), productName: reader["ProductName"].ToString(), title: reader["Title"].ToString(), description: reader["Description"].ToString(), price: Convert.ToSingle(reader["Price"]), quantity: Convert.ToInt32(reader["Quantity"]), productImage: reader["ImageURL"] == DBNull.Value ? null : reader["ImageURL"].ToString(), category: null),
                        quantity: Convert.ToInt32(reader["CartItemQuantity"]),
                        createdat: Convert.ToDateTime(reader["CartCreatedAt"]),
                        updatedat: Convert.ToDateTime(reader["CartUpdatedAt"])
                        ));
                }

                return items;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error while fetching cart items.", ex);
            }
        }
        public static async Task<List<CartItemDTO>> GetCartItemsByID(int id)
        {
            var cartItems = new List<CartItemDTO>();

            await using SqlConnection conn = new SqlConnection(clsSettings.Connection);
            await using SqlCommand cmd = new SqlCommand("sp_GetCartItemsByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CartItemID", id);

            try
            {
                await conn.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var cartItem = new CartItemDTO(
                        cartItemid: Convert.ToInt32(reader["CartItemID"]),
                        cart: new CartDTO(
                            cartid: Convert.ToInt32(reader["CartID"]),
                            userid: Convert.ToInt32(reader["UserID"]),
                            createdAt: Convert.ToDateTime(reader["CreatedAt"]),
                            updatedat: Convert.ToDateTime(reader["UpdatedAt"])
                        ),
                        product: new ProductDTO(
                            productID: Convert.ToInt32(reader["ProductID"]),
                            productName: reader["ProductName"].ToString(),
                            title: reader["Title"].ToString(),
                            description: reader["Description"].ToString(),
                            price: Convert.ToSingle(reader["Price"]),
                            quantity: Convert.ToInt32(reader["Quantity"]),
                            productImage: reader["ImageURL"] == DBNull.Value ? null : reader["ImageURL"].ToString(),
                            category: null
                        ),
                        quantity: Convert.ToInt32(reader["CartItemQuantity"]),
                        createdat: Convert.ToDateTime(reader["CartCreatedAt"]),
                        updatedat: Convert.ToDateTime(reader["CartUpdatedAt"])
                    );

                    cartItems.Add(cartItem);
                }

                return cartItems;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error while fetching cart items.", ex);
            }
        }

        public static (bool success, int cartItemID) CreateCartItem(CartItem_Created cart)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateCartItem", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            // INPUT
            cmd.Parameters.AddWithValue("@UserID", cart.UserID);
            cmd.Parameters.AddWithValue("@ProductID", cart.ProductID);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);

            // OUTPUT CartID
            SqlParameter cartIDParam = new SqlParameter("@CartID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            // OUTPUT CartItemID
            SqlParameter cartItemIDParam = new SqlParameter("@CartItemID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(cartIDParam);
            cmd.Parameters.Add(cartItemIDParam);

            try
            {
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                int cartID = (int)cartIDParam.Value;
                int cartItemID = (int)cartItemIDParam.Value;

                return (cartItemID > 0, cartItemID);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating cart item.", ex);
            }
        }

        public static bool UpdateCartItem(CartItem_Updated cart)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateCartItem", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CartItemID", cart.CartItemID);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            try
            {
                connection.Open();

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Cart Item.", ex);
            }
            finally
            {
                connection.Close();
            }

        }
        public static bool DeleteCartItem(int CartItemID)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_DeleteCartItem", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CartItemID", CartItemID);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Cart Item.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
