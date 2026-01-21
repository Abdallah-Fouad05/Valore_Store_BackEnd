using BAL;
using DAL.DTos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/cart-items")]
    public class CartItemController : ControllerBase
    {
        // GET: api/v1/cart-items
        [HttpGet]
        public async Task<ActionResult<List<CartItemDTO>>> GetAll()
        {
            try
            {
                var items = await CartItemBusiness.GetAllCartItems();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/v1/cart-items/5
        [HttpGet("Get/{cartId:int}")]
        public async Task<ActionResult<List<CartItemDTO>>> GetByCartID(int cartId)
        {
            try
            {
                var items = await CartItemBusiness.GetCartItemByID(cartId);

                if (items == null || items.Count == 0)
                    return NotFound($"No cart items found for Cart ID {cartId}.");

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/v1/cart-items
        [HttpPost]
        public ActionResult Create([FromBody] CartItem_Created cartItem)
        {
            try
            {
                var result = CartItemBusiness.CreateCartItem(cartItem);

                if (!result.success)
                    return BadRequest("CartItem was not created.");

                // redirect to GET By ID
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/v1/cart-items
        [HttpPut("update/")]
        public ActionResult Update([FromBody] CartItem_Updated cartItem)
        {
            try
            {
                bool updated = CartItemBusiness.UpdateCartItem(cartItem);

                if (!updated)
                    return NotFound($"CartItem with ID {cartItem.CartItemID} not found.");

                return Ok("CartItem updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/v1/cart-items/5
        [HttpDelete("Delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool deleted = CartItemBusiness.DeleteCartItem(id);

                if (!deleted)
                    return NotFound($"CartItem with ID {id} not found.");

                return Ok("CartItem deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
