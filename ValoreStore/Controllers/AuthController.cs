using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BAL;
using DAL.DTos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using DAL.DTos.Auth;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.AspNetCore.RateLimiting; //ratelimit
namespace API.Controllers
{
    public class AuthController : Controller
    {
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

       
        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            //var user = StudentDataSimulation.StudentsList
            //    .FirstOrDefault(s => s.Email == request.Email);
            var user = await UserBusiness.GetRefreshTokenByEmail(request.Email.ToString());

            if (user == null)
                return Unauthorized("Invalid refresh request");

            if (user.RefreshTokenRevokedAt != null)
                return Unauthorized("Refresh token is revoked");

            if (user.RefreshTokenExpiresAt == null || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshTokenHash);
            if (!refreshValid)
                return Unauthorized("Invalid refresh token");

            // Issue NEW access token (same claims & signing settings as login)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };



            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new Exception("JWT secret key is missing!");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "ValoreStoreApi",
                audience: "ValoreStoreApiUser",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Rotation: replace refresh token
            var newRefreshToken = GenerateRefreshToken();
            await UserBusiness.UpdateRefreshToken(user.Email, BCrypt.Net.BCrypt.HashPassword(newRefreshToken), DateTime.Now.AddDays(7));

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("Login")]
        [EnableRateLimiting("AuthLimiter")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginRequest result)
        {

            var user = await UserBusiness.LogIn(result);
            if (user == null)
                return Unauthorized("Invalid crediantals.");

            var claims = new[]
                {
                            // Unique identifier for the student
                            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),


                            new Claim(ClaimTypes.Email, user.Email),

                            // Role (Student or Admin) used later for authorization
                            
                            new Claim(ClaimTypes.Role, user.Role)

                       };

            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new Exception("JWT secret key is missing!");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // Step 6: Create the JWT token.
            // The token includes issuer, audience, claims, expiration, and signature.
            var token = new JwtSecurityToken(
                issuer: "ValoreStoreApi",
                audience: "ValoreStoreApiUser",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var refreshToken = GenerateRefreshToken();

            await UserBusiness.UpdateRefreshToken(user.Email, BCrypt.Net.BCrypt.HashPassword(refreshToken), DateTime.Now.AddDays(7));

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
            });


        }

        [HttpPost("SignUp")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(500)]
        public async  Task<IActionResult> SignUp([FromBody] SignInRequest request)
        {
           
                var user = UserBusiness.SignUp(request);

                var claims = new[]
                        {
                            // Unique identifier for the student
                            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),


                            new Claim(ClaimTypes.Email, user.Email),

                            // Role (Student or Admin) used later for authorization

                            new Claim(ClaimTypes.Role, "User")

                       };

                var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

                if (string.IsNullOrWhiteSpace(secretKey))
                    throw new Exception("JWT secret key is missing!");

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                // Step 6: Create the JWT token.
                // The token includes issuer, audience, claims, expiration, and signature.
                var token = new JwtSecurityToken(
                    issuer: "ValoreStoreApi",
                    audience: "ValoreStoreApiUser",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );
            var refreshToken = GenerateRefreshToken();

            await UserBusiness.UpdateRefreshToken(user.Email, BCrypt.Net.BCrypt.HashPassword(refreshToken), DateTime.Now.AddDays(7));


            return Ok(new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken
                });
        }
    }
} 
