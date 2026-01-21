using DAL.DTos;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL
{
    public class clsCartData
    {
        public static async Task<List<CartDTO>> GetAllCarts()
        {
            List<CartDTO> carts = new();

            await using SqlConnection conn = new(clsSettings.Connection);
            await using SqlCommand cmd = new("sp_GetAllCarts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                await conn.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    carts.Add(new CartDTO(
                        Convert.ToInt32(reader["CartID"]),
                        Convert.ToInt32(reader["UserID"]),
                        Convert.ToDateTime(reader["CreatedAt"]),
                        Convert.ToDateTime(reader["UpdatedAt"])
                    ));
                }
                return carts;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Carts.", ex);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public static async Task<CartDTO?> GetCartById(int cartId)
        {
            await using SqlConnection conn = new(clsSettings.Connection);
            await using SqlCommand cmd = new("sp_GetCartById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CartID", cartId);
            try
            {

                await conn.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new CartDTO(
                        Convert.ToInt32(reader["CartID"]),
                        Convert.ToInt32(reader["UserID"]),
                        Convert.ToDateTime(reader["CreatedAt"]),
                        Convert.ToDateTime(reader["UpdatedAt"])
                    );
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the Cart.", ex);
            }
            finally
            {
               await conn.CloseAsync();
            }
        }
        public static (bool success, int id) CreateCart(CartDTO_Created newCart)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new SqlCommand("sp_CreateCart", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // إضافة البراميتر
            cmd.Parameters.AddWithValue("@UserID", newCart.UserID);

            // Output parameter
            SqlParameter outputParam = new SqlParameter("@CartID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                conn.Open();             
                cmd.ExecuteNonQuery();   

                int newID = (int)outputParam.Value;
                return (newID > 0, newID);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the cart.", ex);
            }
        }
        public static bool  UpdateCart(CartDTO cart)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CartID", cart.CartID);
            cmd.Parameters.AddWithValue("@UserID", cart.UserID);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Cart.", ex);
            }
            finally
            {
                conn.Close();
            }
        }
        public static  bool DeleteCart(int cartId)
        {
             using SqlConnection conn = new(clsSettings.Connection);
             using SqlCommand cmd = new("sp_DeleteCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CartID", cartId);
            try
            {
                 conn.Open();
                 int rowEffected = cmd.ExecuteNonQuery();
                return ((int)rowEffected > 0);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Delete Cart.", ex);
            }
            finally
            {
                 conn.Close();
            }
        }
        public static List<CartItemDetails> GetCartItems(int userID)
        {
            List<CartItemDetails> items = new List<CartItemDetails>();

            using SqlConnection conn = new SqlConnection(clsSettings.Connection);
            using SqlCommand cmd = new SqlCommand("SP_GetCartItems", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CartItemDetails item = new CartItemDetails(
                        cartItemID: reader.GetInt32(reader.GetOrdinal("CartItemID")),
                        cartID: reader.GetInt32(reader.GetOrdinal("CartID")),
                        productID: reader.GetInt32(reader.GetOrdinal("ProductID")),
                        productName: reader.GetString(reader.GetOrdinal("ProductName")),
                        quantity: reader.GetInt32(reader.GetOrdinal("Quantity")),
                        price: reader.GetFloat(reader.GetOrdinal("Price")),
                        totalPrice: reader.GetFloat(reader.GetOrdinal("TotalPrice")),
                        imageURL: reader.GetString(reader.GetOrdinal("ImageURL"))
                          );

                    items.Add(item);
                }

                return items;
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while retrieving basket items.", ex);
            }
        }
    }
}
