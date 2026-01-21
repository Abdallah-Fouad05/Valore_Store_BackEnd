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
    public class clsCategoryData
    {
        public static async Task<List<CategoryDTO>> GetAllCategory()
        {
            List<CategoryDTO> Categories = new();

            await using SqlConnection connection = new(clsSettings.Connection);
            await using SqlCommand cmd = new("sp_GetAllCategory", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Categories.Add(new CategoryDTO(
                            Convert.ToInt32(reader["CategoryID"]),
                            reader["CategoryName"].ToString(),
                            reader["ImageURL"].ToString()
                        )
                    );
                }
                return Categories;
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
        public static async Task<CategoryDTO> GetCategoryByID(int CategoryID)
        {
           

            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand cmd = new SqlCommand("sp_GetCategoryByID", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    CategoryDTO Categories = new CategoryDTO(
                            Convert.ToInt32(reader["CategoryID"]),
                            reader["CategoryName"].ToString(),
                            reader["ImageURL"].ToString()
                        );
                    return Categories;
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
        public static (bool success, int CategoryID) CreateCategory(CategoryDTO category)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateCategory", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            cmd.Parameters.AddWithValue("@CategoryImage", category.CategoryImage);


            SqlParameter outputParam = new SqlParameter("@CategoryID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            try
            {
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return (rowsAffected > 0, (int)outputParam.Value);
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
        public static bool UpdateCategory(CategoryDTO category)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateCategory", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            cmd.Parameters.AddWithValue("@CategoryImage", category.CategoryImage);
            cmd.Parameters.AddWithValue("@CategoryID", category.CategoryID);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Category.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool DeleteCategory(int categoryID)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_DeleteCategory", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Delete Category.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
