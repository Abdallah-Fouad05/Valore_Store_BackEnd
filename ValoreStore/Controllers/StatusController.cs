using BAL;
using DAL.DTos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        // GET: api/v1/status
        [HttpGet]
        [ProducesResponseType(typeof(List<StatusDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var statuses = await StatusBusiness.GetAllStatus();
                return Ok(statuses);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/v1/status/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StatusDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var status = await StatusBusiness.GetStatusByID(id);

                if (status == null)
                    return NotFound($"Status with ID {id} not found.");

                return Ok(status);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/v1/status
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public IActionResult Create([FromBody] StatusDTO status)
        {
            try
            {
                var (success, statusID) = StatusBusiness.CreateStatus(status);

                if (!success)
                    return BadRequest("Status creation failed.");

                return CreatedAtAction(nameof(GetByID), new { id = statusID }, new { StatusID = statusID });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/v1/status
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Update([FromBody] StatusDTO status)
        {
            try
            {
                bool updated = StatusBusiness.UpdateStatus(status);

                if (!updated)
                    return NotFound($"Status with ID {status.StatusID} not found.");

                return Ok(new { message = "Status updated successfully." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/v1/status/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Delete(int id)
        {
            try
            {
                bool deleted = StatusBusiness.DeleteStatus(id);

                if (!deleted)
                    return NotFound($"Status with ID {id} not found.");

                return Ok(new { message = "Status deleted successfully." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
