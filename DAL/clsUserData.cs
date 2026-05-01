using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DAL.DTos;
using DAL.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL
{
    public class clsUserData
    {
        public static async Task<List<UserDTO>> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();

            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand command = new SqlCommand("Select * from Users;", connection);

            try
            {
                await connection.OpenAsync();

                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDTO(
                        Convert.ToInt32(reader["UserID"]),
                        reader["UserName"].ToString(),
                        reader["Email"].ToString(),
                        null, // Password (مش بنرجعه غالباً)
                        reader["ImageURL"] == DBNull.Value ? null : reader["ImageURL"].ToString(),
                        Convert.ToDateTime(reader["CreatedAt"]),
                        reader["UpdatedAt"] == DBNull.Value ? DateTime.Now: Convert.ToDateTime(reader["UpdatedAt"]),
                        Convert.ToBoolean(reader["IsAdmin"]) ? "Admin" : "User"
                    ));
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        public static async Task<UserDTO> GetUserByID(int userID)
        {

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
                        Convert.ToBoolean(reader["IsAdmin"])? "Admin" : "User"
                        );
                }
                return null;
            }
            catch (Exception ex)
            {
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
            cmd.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(user.Password));
            cmd.Parameters.AddWithValue("@ImageURL", (object?)user.ImageURL ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsAdmin", user.Role == "Admin" ? true : false);

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
            cmd.Parameters.AddWithValue("@ImageURL", (object?)user.ImageURL ?? DBNull.Value);

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

        public static bool ChangePassword(UserDTO user)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateUser", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", user.UserID);
            cmd.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(user.Password));
          
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating password.", ex);
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



        public static async Task<UserDTO> LogIn(LoginRequest request)
        {
            await using SqlConnection connection = new(clsSettings.Connection);
            await using SqlCommand command = new("sp_LogIn", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email", request.Email);
            
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {

                    var user = new UserDTO
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = Convert.ToBoolean(reader["IsAdmin"]) ? "Admin" : "User"
                    };

                    bool isValidPassword =
                        BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

                    if (!isValidPassword)
                        return null;



                    return user;



                }
                return null;
          
        }
        public static UserDTO CreateUser(SignInRequest user)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_SignUp", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@fullName", user.FullName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(user.Password));
            cmd.Parameters.AddWithValue("@createdat", DateTime.Now);

            try
            {
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new UserDTO
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

        public static async Task<bool> ChangeUserRole(int adminId, int userId, bool role)
        {
            using (SqlConnection conn = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand cmd = new SqlCommand("ChangeRole", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Role", role);

                    
                    var returnParam = new SqlParameter();
                    returnParam.Direction = ParameterDirection.ReturnValue;

                    cmd.Parameters.Add(returnParam);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    int result = (int)returnParam.Value;

                    return result == 1;
                }
            }
        }


        public static async Task<UserDTO?> GetRefreshTokenByEmail(string email)
        {
            const string query = @"
            SELECT UserID,Email,IsAdmin, RefreshTokenHash, RefreshTokenExpiresAt, RefreshTokenRevokedAt
            FROM Users
            WHERE Email = @Email;";

            await using SqlConnection connection = new(clsSettings.Connection);
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = email;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new UserDTO(
                            Convert.ToInt16(reader["UserID"]),
                            reader["Email"] as string,
                            Convert.ToBoolean(reader["IsAdmin"])? "Admin" : "User",
                            reader["RefreshTokenHash"] as string,
                            reader["RefreshTokenExpiresAt"] as DateTime?,
                            reader["RefreshTokenRevokedAt"] as DateTime?
                    );
                    }
                    return null;
                }
            }


        }

        public static async Task<bool> UpdateRefreshToken(string email, string refreshTokenHash, DateTime expiresAt)
        {
            const string query = @"
UPDATE Users
SET 
    RefreshTokenHash = @RefreshTokenHash,
    RefreshTokenExpiresAt = @RefreshTokenExpiresAt,
    RefreshTokenRevokedAt = NULL
OUTPUT inserted.UserID
WHERE Email = @Email;
";

            await using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@RefreshTokenHash", refreshTokenHash);
            cmd.Parameters.AddWithValue("@RefreshTokenExpiresAt", expiresAt);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result != null; // true لو حصل تحديث
        }
    }
}