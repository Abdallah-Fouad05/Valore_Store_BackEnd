using BAL;
using DAL;
using DAL.DTos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
      

        // POST: api/OrderItems
        [HttpPost]
        public IActionResult Create([FromBody] OrderItemDTO_Created orderItem)
        {
            if (orderItem == null)
                return BadRequest("OrderItem object is null.");

            try
            {
                var result = OrderItemBusiness.CreateOrderItem(orderItem);
                if (result.success)
                    return Ok(new { OrderItemID = result.orderItemID });
                else
                    return StatusCode(500, "Failed to create order item.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] OrderItemDTO_Updated orderItem)
        {
            if (orderItem == null || id != orderItem.OrderItemID)
                return BadRequest("OrderItem object is null or ID mismatch.");

            try
            {
                var success = OrderItemBusiness.UpdateOrderItem(orderItem);
                if (success)
                    return NoContent(); // 204
                else
                    return StatusCode(500, "Failed to update order item.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var success = OrderItemBusiness.DeleteOrderItem(id);
                if (success)
                    return NoContent(); // 204
                else
                    return StatusCode(500, $"Failed to delete order item with ID {id}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
