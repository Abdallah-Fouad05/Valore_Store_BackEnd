using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTos; 
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await ProductBusiness.Find(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found.");

                return Ok(product);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CreateProduct([FromBody] Product_Created product)
        {
            try
            {
                var (success, productId) = ProductBusiness.CreateProduct(product);
                if (success)
                    return StatusCode(201, new { ProductID = productId});

                return StatusCode(500, new { message = "Product creation failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }




        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult UpdateProduct([FromBody] Product_Created product)
        {
            try
            {
                var result = ProductBusiness.UpdateProduct(product);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "Product update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var result = ProductBusiness.DeleteProduct(id);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "Product deletion failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(List<ProductDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await ProductBusiness.GetAllProducts();
                return Ok(products);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("ByCategory/{categoryID}")]
        [ProducesResponseType(typeof(List<ProductDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllProductsByCategory(int categoryID)
        {
            try
            {
                var products = await ProductBusiness.GetAllProductByCategoryID(categoryID);
                return Ok(products);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("Random/{count}")]
        [ProducesResponseType(typeof(List<ProductDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetRandomProducts(int count)
        {
            try
            {
                var allProducts = await ProductBusiness.GetAllProducts();
                var random = new Random();
                var products = allProducts.OrderBy(x => random.Next()).Take(count).ToList();
                return Ok(products);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
