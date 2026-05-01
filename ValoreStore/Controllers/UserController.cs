using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTos;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var Users = await UserBusiness.GetAllUsers();
                return Ok(Users);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(int id, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var user = await UserBusiness.GetUserByID(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                var authResult = await authorizationService.AuthorizeAsync(
               User,
               id,
               "UserOwnerOrAdmin");

                //poilecey
                if (!authResult.Succeeded)
                    return Forbid(); // 403

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

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser([FromForm] User_Updated_Request user)
        {
            try
            {
                string? fileName = null;
                if (user.UserImage != null && user.UserImage.Length > 0)
                {
                    fileName = $"{Guid.NewGuid()}{Path.GetExtension(user.UserImage.FileName)}";
                    var path = Path.Combine("wwwroot/images", fileName);

                    using var stream = new FileStream(path, FileMode.Create);

                    await user.UserImage.CopyToAsync(stream);
                }
                var updated_user = new UserDTO
                (
                   user.UserID,
                  user.UserName, "", "", fileName, DateTime.Now, DateTime.Now, ""
                );

                var result = UserBusiness.UpdateUser(updated_user);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "Product update failed." });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("ChangePassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ChangePassword([FromBody] User_ChangePassword_Request user)
        {
            try
            {
               
                var updated_user = new UserDTO
                (
                   user.UserID,"","",user.password,
                   "", DateTime.Now, DateTime.Now, ""
                );

                var result = UserBusiness.ChangePassword(updated_user);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "Product change Password erro." });
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

        [HttpPut("ChangeRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ChangeUserRole(User_ChangeRole_Request user)
        {
            try
            {
                var result = await UserBusiness.ChangeUserRole(user.AdminID,user.UserID,user.Role);
                if (result)
                    return Ok();

                return StatusCode(500, new { message = "User updatin failed." });
            }
            catch (ApplicationException ex) { 
                   return StatusCode(500, new {message = ex.Message});            
            }
        }
    }
}