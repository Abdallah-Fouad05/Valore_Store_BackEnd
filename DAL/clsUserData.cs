using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTos;
using DAL.DTOs;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL
{
    public class clsUserData
    {
        public static async Task<UserDTO> GetUserByID(int userID) {

            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand command = new SqlCommand("sp_GetUserByID", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", userID);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.ReadAsync() != null)
                {
                    return new UserDTO(
                        Convert.ToInt32(reader["UserID"]),
                        reader["UserName"].ToString(),
                        reader["Email"].ToString(),
                        null,
                        reader["ImageURL"].ToString(),
                        Convert.ToDateTime(reader["CreatedAt"]),
                        Convert.ToDateTime(reader["CreatedAt"]),
                        Convert.ToBoolean(reader["IsAdmin"])
                        );
                }
                return null;
            }
            catch (Exception ex) { 
                     throw new Exception("An error occurred while retrieving User.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        public static (bool success, int userID) CreateUser(UserDTO user)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateUser", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@ImageURL", (object?)user.ImageURL ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

            SqlParameter outputParam = new SqlParameter("@UserID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return ((int)outputParam.Value > 0, (int)outputParam.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool UpdateUser(UserDTO user)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateUser", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", user.UserID);
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", (object?)user.Password ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ImageURL", (object?)user.ImageURL ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating User.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool DeleteUser(int userID)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_DeleteUser", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Delete User.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static async Task<UserLogInResult> LogIn(UserLogInDTO user)
        {
            await using SqlConnection connection = new(clsSettings.Connection);
            await using SqlCommand command = new("sp_LogIn", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email",user.Email);
            command.Parameters.AddWithValue("@password", user.Password);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.ReadAsync() != null)
                {
                    return new UserLogInResult
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Log in.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        public static CreatedUser CreateUser(UserSignUpDTO user)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_SignUp", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@fullName", user.FullName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@createdat", DateTime.Now);

            try
            {
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new CreatedUser
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Email = reader["Email"].ToString()!
                    };
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

    }
}
