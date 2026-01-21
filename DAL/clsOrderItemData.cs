using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTos;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class clsOrderItemData
    {

        // Create order item
        public static (bool success, int orderItemID) CreateOrderItem(OrderItemDTO_Created orderItem)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new SqlCommand("sp_CreateOrderItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
            cmd.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
            cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            cmd.Parameters.AddWithValue("@Address", orderItem.Address);
            cmd.Parameters.AddWithValue("@StatusID", orderItem.StatusID);

            SqlParameter outputParam = new SqlParameter("@OrderItemID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return ((int)outputParam.Value > 0, (int)outputParam.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the order item.", ex);
            }
            finally
            {
                conn.Close();
            }
        }

        // Update order item
        public static bool UpdateOrderItem(OrderItemDTO_Updated orderItem)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new SqlCommand("sp_UpdateOrderItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderItemID", orderItem.OrderItemID);
            cmd.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
            cmd.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
            cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            cmd.Parameters.AddWithValue("@Address", orderItem.Address);
            cmd.Parameters.AddWithValue("@StatusID", orderItem.StatusID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the order item.", ex);
            }
            finally
            {
                conn.Close();
            }
        }

        // Delete order item
        public static bool DeleteOrderItem(int orderItemID)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new SqlCommand("sp_DeleteOrderItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderItemID", orderItemID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the order item.", ex);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
