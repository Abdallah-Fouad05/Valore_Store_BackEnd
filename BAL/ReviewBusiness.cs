using DAL;
using DAL.DTos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public class ReviewBusiness
    {
        public static async Task<List<ReviewDatailsDTO>> GetAllReviews()
        {
            try
            {
                clsReviewData data = new clsReviewData();
                return await data.GetAllReview();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all reviews.", ex);
            }
        }
        public static (bool success, int reviewID) CreateReview(ReviewDTO review)
        {
            try
            {
                return clsReviewData.CreateReview(review);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating review.", ex);
            }
        }
        public static bool UpdateReview(ReviewDTO review)
        {
            try
            {
                return clsReviewData.UpdateReview(review);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while updating review with ID {review.ReviewID}.", ex);
            }
        }
        public static async Task<bool> DeleteReview(int reviewID)
        {
            try
            {
                return await clsReviewData.DeleteReview(reviewID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while deleting review with ID {reviewID}.", ex);
            }
        }
        public static async Task<List<ReviewDTO>> GetReviewsByUserID(int userID)
        {
            try
            {
                return await clsReviewData.GetReviewsByUserID(userID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching reviews for user ID {userID}.", ex);
            }
        }
        public static async Task<List<ReviewDTO>> GetReviewsByProductID(int productID)
        {
            try
            {
                return await clsReviewData.GetReviewsByProductID(productID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching reviews for product ID {productID}.", ex);
            }
        }
    }
}
