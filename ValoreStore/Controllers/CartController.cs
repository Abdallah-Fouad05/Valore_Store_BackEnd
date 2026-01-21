using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        

        // ================== Get Cart By ID ==================
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CartDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetCartById(int id)
        {
            try
            {
                var cart = await CartBusiness.GetCartById(id);
                if (cart == null)
                    return NotFound($"Cart with ID {id} not found.");

                return Ok(cart);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ================== Create Cart ==================
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult CreateCart([FromBody] CartDTO_Created cart)
        {
            try
            {
                var (success, cartId) = CartBusiness.CreateCart(cart);
                if (success)
                    return StatusCode(201, new { CartID = cartId });

                return StatusCode(500, new { message = "Cart creation failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ================== Update Cart ==================
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult UpdateCart([FromBody] CartDTO cart)
        {
            try
            {
                var result = CartBusiness.UpdateCart(cart);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "Cart update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ================== Delete Cart ==================
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult DeleteCart(int id)
        {
            try
            {
                var result = CartBusiness.DeleteCart(id);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "Cart deletion failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ================== Get All Carts ==================
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(List<CartDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllCarts()
        {
            try
            {
                var carts = await CartBusiness.GetAllCarts();
                return Ok(carts);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //==================================================
        [HttpGet("{userID:int}")]
        [ProducesResponseType(typeof(List<CartItemDetails>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetCartItems(int userID)
        {
            try
            {
                var items = CartBusiness.GetCartItems(userID);

                if (items == null || items.Count == 0)
                    return NotFound($"No cart items found for user ID {userID}.");

                return Ok(items);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
