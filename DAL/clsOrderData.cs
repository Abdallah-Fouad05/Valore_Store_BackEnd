using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTos;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class clsOrderData
    {
        public static async Task<orderDTO_Details> GetOrdersByUserID(int userID)
        {

            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand cmd = new SqlCommand("sp_GetOrdersByUserID", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    return new orderDTO_Details(
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                new UserDTO(reader.GetInt32(reader.GetOrdinal("UserID")), reader.GetString(reader.GetOrdinal("UserName")), null, null, reader.GetString(reader.GetOrdinal("ImageUrl")), DateTime.Now, DateTime.Now, false),
                                reader.GetDateTime(reader.GetOrdinal("OrderDate")), reader.GetFloat(reader.GetOrdinal("TotalAmount")));
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the orders.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        public static (bool success, int OrderID) CreateOrder(OrderDTO_Created order)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateOrder", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", order.UserID);

            SqlParameter outputParam = new SqlParameter("@OrderID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return ((int)outputParam.Value > 0, (int)outputParam.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the Order.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool UpdateOrder(OrderDTO_Updated order)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateOrder", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
            cmd.Parameters.AddWithValue("@UserID", order.UserID);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Order.", ex);
            }
            finally
            {
                connection.Close();
            }
        }


        // 
        public static List<OrderItemDTO_Datelis> GetOrderItems(int userID)
        {
            List<OrderItemDTO_Datelis> items = new List<OrderItemDTO_Datelis>();

            using SqlConnection conn = new SqlConnection(clsSettings.Connection);
            using SqlCommand cmd = new SqlCommand("sp_GetOrderItems", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderItemDTO_Datelis item = new OrderItemDTO_Datelis(
                        orderitemid: reader.GetInt32(reader.GetOrdinal("OrderItemID")),
                        order: new orderDTO(
                               orderid: reader.GetInt32(reader.GetOrdinal("OrderID")),
                               user: new UserDTO(reader.GetInt32(reader.GetOrdinal("UserID")),reader.GetString(reader.GetOrdinal("UserName")),"",null, reader.GetString(reader.GetOrdinal("UserImageURL")),DateTime.Now,DateTime.Now,false),
                               createdat: reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                               totalamount: reader.GetFloat(reader.GetOrdinal("TotalAmount"))),
                        product: new ProductDTO(reader.GetInt32(reader.GetOrdinal("ProductID")),reader.GetString(reader.GetOrdinal("ProductName")),"","",0,0,reader.GetString(reader.GetOrdinal("ProductImageURL")),null),
                           status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusID")),reader.GetString(reader.GetOrdinal("StatusName"))),
                        quantity: reader.GetInt32(reader.GetOrdinal("Quantity")),
                        price: reader.GetFloat(reader.GetOrdinal("Price")),
                     address: reader.GetString(reader.GetOrdinal("Address")));

                    items.Add(item);
                }

                return items;
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while retrieving order item.", ex);
            }
        }

    }
}
