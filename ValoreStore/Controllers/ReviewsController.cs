using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Review")]
    public class ReviewController : ControllerBase
    {
        // GET: api/v1/Review/GetAll
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(List<ReviewDatailsDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await ReviewBusiness.GetAllReviews();
                return Ok(reviews);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/v1/Review/User/5
        [HttpGet("User/{userID:int}")]
        [ProducesResponseType(typeof(List<ReviewDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewsByUserID(int userID)
        {
            try
            {
                var reviews = await ReviewBusiness.GetReviewsByUserID(userID);
                return Ok(reviews);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/v1/Review/Product/3
        [HttpGet("Product/{productID:int}")]
        [ProducesResponseType(typeof(List<ReviewDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetReviewsByProductID(int productID)
        {
            try
            {
                var reviews = await ReviewBusiness.GetReviewsByProductID(productID);
                return Ok(reviews);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/v1/Review
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromBody] ReviewDTO review)
        {
            try
            {
                var (success, reviewID) = ReviewBusiness.CreateReview(review);
                if (!success)
                    return StatusCode(500, new { message = "Review creation failed." });

                return StatusCode(201, new { ReviewID = reviewID });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/v1/Review
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview([FromBody] ReviewDTO review)
        {
            try
            {
                var success = ReviewBusiness.UpdateReview(review);
                if (success)
                    return Ok();

                return StatusCode(500, new { message = "Review update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/v1/Review/5
        [HttpDelete("{reviewID:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteReview(int reviewID)
        {
            try
            {
                var success = await ReviewBusiness.DeleteReview(reviewID);
                if (success)
                    return Ok();

                return StatusCode(500, new { message = "Review deletion failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
