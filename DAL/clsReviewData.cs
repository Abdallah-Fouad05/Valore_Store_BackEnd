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
    public class clsReviewData
    {
        public async Task<List<ReviewDatailsDTO>> GetAllReview()
        {
            List<ReviewDatailsDTO> reviews = new();
            await using SqlConnection connection = new SqlConnection(clsSettings.Connection);
            await using SqlCommand command = new SqlCommand("sp_GetAllReviews",connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    reviews.Add(new ReviewDatailsDTO(
                            Convert.ToInt32(reader["ReviewID"]),
                            reader["UserName"].ToString(),
                            reader["UserImage"] == DBNull.Value ? null : reader["UserImage"].ToString(),
                            reader["ProductName"].ToString(),
                            reader["ProductImage"] == DBNull.Value ? null : reader["ProductImage"].ToString(),
                            reader["Title"].ToString(),
                            Convert.ToInt32(reader["Rating"]),
                            reader["Comment"].ToString(),
                            Convert.ToDateTime(reader["CreatedAt"])
                     ));


                }
                return reviews;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error while fatching Reviews.", ex);
            }
            finally
            {
                 await connection.CloseAsync();
            }
        }
        public static  (bool success, int ID) CreateReview(ReviewDTO review)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_CreateReview", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", review.UsertID);
            cmd.Parameters.AddWithValue("@ProductID", review.ProductID);
            cmd.Parameters.AddWithValue("@Rating", review.Ratting);
            cmd.Parameters.AddWithValue("@Comment", review.Comment);

             SqlParameter outputParam = new SqlParameter("@ReviewID", SqlDbType.Int)
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
                throw new Exception("An error occurred while creating the Review.", ex);
            }
            finally
            {
                conn.Close();
            }
        }   
        public static bool UpdateReview(ReviewDTO review)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_UpdateReview", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ReviewID", review.ReviewID);
            cmd.Parameters.AddWithValue("@Rating", review.Ratting);
            cmd.Parameters.AddWithValue("@Comment", review.Comment);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Review.", ex);
            }
            finally
            {
                conn.Close();
            }
        }
        public static async Task<bool> DeleteReview(int reviewID)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_DeleteReview", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ReviewID", reviewID);
             try
             {
                 conn.Open();
                 cmd.ExecuteNonQuery();
                 return true;
             }
                
              catch (Exception ex)
             {
                 throw new Exception("An error occurred while Delete the Review.", ex);
             }
             finally
             {
                 conn.Close();
             }
        }
        public static async Task<List<ReviewDTO>> GetReviewsByUserID(int userID)
        {
            List<ReviewDTO> reviews = new();

            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_GetAllReviewsByUserID", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                await conn.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    reviews.Add(new ReviewDTO(Convert.ToInt32(reader["ReviewID"]), Convert.ToInt32(reader["UserID"]), Convert.ToInt32(reader["ProductID"]), Convert.ToSingle(reader["Rating"]), reader["Comment"].ToString(), Convert.ToDateTime(reader["CreatedAt"])));
                    return reviews;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fatching the Reviews.", ex);
            }
            finally
            {
                conn.Close();
            }
        }
        public static async Task<List<ReviewDTO>> GetReviewsByProductID(int productID)
        {
            List<ReviewDTO> reviews = new();

            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("sp_GetAllReviewsByProductID", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductID", productID);

            try
            {
                await conn.OpenAsync();
              
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    reviews.Add(new ReviewDTO(Convert.ToInt32(reader["ReviewID"]), Convert.ToInt32(reader["UserID"]), Convert.ToInt32(reader["ProductID"]), Convert.ToSingle(reader["Rating"]), reader["Comment"].ToString(), Convert.ToDateTime(reader["CreatedAt"])));
                }
                return reviews;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fatching the Reviews.", ex);
            }
            finally
            {
                conn.Close();
            }

        }

    }
}

