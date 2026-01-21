using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTos; // تأكد الاسم صحيح CategoryDTO
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await CategoryBusiness.Find(id);
                if (category == null)
                    return NotFound($"Category with ID {id} not found.");

                return Ok(category);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CreateCategory([FromBody] CategoryDTO category)
        {
            try
            {
                var (success, categoryID) = CategoryBusiness.CreateCategory(category);
                if (success)
                {
                    return StatusCode(201, new { CategoryID = categoryID });
                }

                return StatusCode(500, new { message = "Category creation failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult UpdateCategory([FromBody] CategoryDTO category)
        {
            try
            {
                var result = CategoryBusiness.UpdateCategory(category);
                if (result)
                    return Ok(new { message = $"Category with ID {category.CategoryID} was updated successfully.", category });


                return StatusCode(500, new { message = "Category update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var result = CategoryBusiness.DeleteCategory(id);
                if (result)
                    return Ok(new { message = $"Category with ID {id} was deleted successfully." });

                return StatusCode(500, new { message = "Category deletion failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(List<CategoryDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await CategoryBusiness.GetAllCategories();
                return Ok(categories);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
