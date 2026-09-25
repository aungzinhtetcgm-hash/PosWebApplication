using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PosWebApplication.Controllers.Api;
using PosWebApplication.DTOs.User;
using PosWebApplication.Services.User;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PosWebApplication.Controllers.Api
{
    [Route("api/user")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserApiController(
            IUserService userService,
            IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(
            [FromBody] RegisterDTO model)
        {
            var result =
                _userService.Register(model);

            if (!result)
            {
                return SendError(
                    "Email already exists.",
                    null,
                    HttpStatusCode.Conflict);
            }

            return SendResponse<object?>(
                null,
                "User registered successfully.",
                HttpStatusCode.Created);
        }

        [HttpPost("login")]
        public IActionResult Login(
            [FromBody] LoginDTO model)
        {
            var user =
                _userService.Login(model);

            if (user == null)
            {
                return SendError(
                    "Invalid email or password.",
                    null,
                    HttpStatusCode.Unauthorized);
            }

            var token =
                GenerateToken(user);

            var userResponse = new
            {
                u_id = user.u_id,
                email = user.email,
                name = user.name,
                role = user.role
            };

            return SendResponse(
                new
                {
                    accessToken = token,
                    user = userResponse
                },
                "Login successful.",
                HttpStatusCode.OK);
        }

        [Authorize(
            AuthenticationSchemes =
                JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return SendResponse<object?>(
                null,
                "Logout successful.",
                HttpStatusCode.OK);
        }

        private string GenerateToken(
            PosWebApplication.Entity.user user)
        {
            var jwtKey =
                _configuration["JWT:Key"];

            var jwtIssuer =
                _configuration["JWT:Issuer"];

            var jwtAudience =
                _configuration["JWT:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "JWT:Key is missing in appsettings.json.");
            }

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new InvalidOperationException(
                    "JWT:Issuer is missing in appsettings.json.");
            }

            if (string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new InvalidOperationException(
                    "JWT:Audience is missing in appsettings.json.");
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.u_id.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.email ?? string.Empty),

                new Claim(
                    ClaimTypes.Name,
                    user.name ?? string.Empty),

                new Claim(
                    ClaimTypes.Role,
                    user.role.ToString())
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}