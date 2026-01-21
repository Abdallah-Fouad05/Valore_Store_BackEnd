using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTos;
using DAL.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await UserBusiness.GetUserByID(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                return Ok(user);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            try
            {
                var (success, userID) = UserBusiness.CreateUser(user);
                if (!success)
                    return StatusCode(500, new { message = "User creation failed." });

                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = userID },
                    user
                );
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateUser([FromBody] UserDTO user)
        {
            try
            {
                var result = UserBusiness.UpdateUser(user);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "User update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var result = UserBusiness.DeleteUser(id);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "User deletion failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserLogInResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] UserLogInDTO user)
        {
            try
            {
                var result = await UserBusiness.LogIn(user);
                if (result == null)
                    return Unauthorized("Invalid email or password.");

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("SignUp")]
        [ProducesResponseType(typeof(CreatedUser), 201)]
        [ProducesResponseType(500)]
        public IActionResult SignUp([FromBody] UserSignUpDTO user)
        {
            try
            {
                var createdUser = UserBusiness.SignUp(user);

                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = createdUser.UserID},
                    createdUser
                );
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
