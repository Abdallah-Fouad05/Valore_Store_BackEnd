using System.Data;
using System.Reflection.Metadata;
using DAL.DTos;
using Microsoft.Data.SqlClient;
namespace DAL
{
    public class clsProdcutData
    {
        public static async Task<List<ProductDTO>> GetAllProduct()
        {
            List<ProductDTO> Products = new();

            await using SqlConnection connection = new(clsSettings.Connection);
            await using SqlCommand cmd = new("sp_GetAllProduct", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Products.Add(new ProductDTO(
                        Convert.ToInt32(reader["ProductID"]),
                        reader["ProductName"].ToString(),
                        reader["Title"].ToString(),
                        reader["Description"].ToString(),
                        Convert.ToSingle(reader["Price"]),
                        Convert.ToInt32(reader["Quantity"]),
                        reader["ProductImage"].ToString(),
                        new CategoryDTO(
                            Convert.ToInt32(reader["CategoryID"]),
                            reader["CategoryName"].ToString(),
                            reader["CategoryImage"].ToString()
                        )
                    ));
                }
                return Products;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while retrieving the product.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }


        }
        public static async Task<ProductDTO> GetProductByID(int prodcutID)
        {
            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand cmd = new SqlCommand("sp_GetProductByID", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", prodcutID);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ProductDTO(
                        Convert.ToInt32(reader["ProductID"]),
                        reader["ProductName"].ToString(),
                        reader["Title"].ToString(),
                        reader["Description"].ToString(),
                        Convert.ToSingle(reader["Price"]),
                        Convert.ToInt32(reader["Quantity"]),
                        reader["ImageURL"].ToString(),
                        new CategoryDTO(
                            Convert.ToInt32(reader["CategoryID"]),
                            reader["CategoryName"].ToString(),
                            null
                        )
                    );
                }
                return null;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while retrieving the product.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        public static async Task<List<ProductDTO>> GetAllProductCategory(int categoryID)
        {
            List<ProductDTO> Products = new();

            await using SqlConnection connection = new(clsSettings.Connection);
            await using SqlCommand cmd = new("sp_GetAllProductCategory", connection);
            cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Products.Add(new ProductDTO(
                        Convert.ToInt32(reader["ProductID"]),
                        reader["ProductName"].ToString(),
                        reader["Title"].ToString(),
                        reader["Description"].ToString(),
                        Convert.ToSingle(reader["Price"]),
                        Convert.ToInt32(reader["Quantity"]),
                        reader["ProductImage"].ToString(),
                        new CategoryDTO(
                            Convert.ToInt32(reader["CategoryID"]),
                            reader["CategoryName"].ToString(),
                            reader["CategoryImage"].ToString()
                        )
                    ));
                }
                return Products;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the product.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }


        }
        public static (bool success, int productID) CreateProduct(Product_Created product)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateProduct", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Title", product.Title);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@ProductImage", product.ProductImage ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);

            SqlParameter outputParam = new SqlParameter("@ProductID", SqlDbType.Int)
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
                throw new Exception("An error occurred while creating the product.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool UpdateProduct(Product_Created product)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateProduct", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID",product.ProductID);
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Title", product.Title);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@ProductImage", product.ProductImage ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Product.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool DeleteProduct(int productID)
        {
            using SqlConnection connection = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_DeleteProduct", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", productID);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Delete Product.", ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
