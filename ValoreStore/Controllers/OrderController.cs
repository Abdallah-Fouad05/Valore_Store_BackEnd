using BAL;
using DAL.DTos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        // GET: api/v1/orders/user/5
        [HttpGet("user/{userID:int}")]
        [ProducesResponseType(typeof(orderDTO_Details), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrdersByUserID(int userID)
        {
            try
            {
                var order = await OrderBusiness.GetOrdersByUserID(userID);

                if (order == null)
                    return NotFound($"No orders found for user ID {userID}.");

                return Ok(order);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/v1/orders
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public IActionResult CreateOrder([FromBody] OrderDTO_Created order)
        {
            try
            {
                var (success, orderID) = OrderBusiness.CreateOrder(order);

                if (!success)
                    return BadRequest("Order creation failed.");

                return CreatedAtAction(
                    nameof(GetOrdersByUserID),
                    new { userID = order.UserID },
                    new { OrderID = orderID }
                );
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/v1/orders/status
        [HttpPut("status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateOrderStatus([FromBody] OrderDTO_Updated order)
        {
            try
            {
                bool updated = OrderBusiness.UpdateOrderStatus(order);

                if (!updated)
                    return NotFound($"Order with ID {order.OrderID} not found.");

                return Ok(new { message = "Order status updated successfully." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{userID:int}")]
        [ProducesResponseType(typeof(List<CartItemDetails>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderItems(int userID)
        {
            try
            {
                var items = OrderBusiness.GetOrderItems(userID);

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
