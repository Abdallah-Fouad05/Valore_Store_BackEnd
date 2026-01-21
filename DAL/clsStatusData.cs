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
    public class clsStatusData
    {
        public static async Task<List<StatusDTO>> GetAllStatus()
        {
            List<StatusDTO> statuses = new();

            await using SqlConnection connection = new(clsSettings.Connection);
            await using SqlCommand cmd = new("sp_GetAllStatus", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    statuses.Add(new StatusDTO(
                            Convert.ToInt32(reader["StatusID"]),
                            reader["StatusName"].ToString()
                        )
                    );
                }
                return statuses;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the status.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }


        }
        public static async Task<StatusDTO> GetStatusByID(int statusID)
        {


            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand cmd = new SqlCommand("sp_GetStatusByID", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StatusID",statusID);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    StatusDTO statuses = new StatusDTO(
                            Convert.ToInt32(reader["StatusID"]),
                            reader["StatusName"].ToString()
                        );
                    return statuses;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Categories.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        public static (bool success, int CategoryID) CreateStatus(StatusDTO status)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateStatus", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StatusName", status.StatusName);


            SqlParameter outputParam = new SqlParameter("@StatusID", SqlDbType.Int)
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
                throw new Exception("An error occurred while creating the Category.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool UpdateStatus(StatusDTO status)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateStatus", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StatusName", status.StatusName);
            cmd.Parameters.AddWithValue("@StatusID", status.StatusID);
            connection.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        public static bool DeleteStatus(int statusid)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_DeleteStatus", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StatusID",statusid );

            connection.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
    }
}
