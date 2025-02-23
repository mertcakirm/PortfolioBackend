using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [ApiController]
    [Route("api/auth")]    
    [Authorize]

    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthController(UserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var isValidUser = _userRepository.ValidateUser(loginRequest.Username, loginRequest.Password);

            if (!isValidUser)
            {
            return Unauthorized(new { message = "Invalid username or password." });
                
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, loginRequest.Username!),
                new("Role", loginRequest.RoleId.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
                Issuer = _config["JwtSettings:Issuer"]!,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return Ok(jwt);

        }

        [HttpPost("add")]
        public IActionResult AddUser([FromBody] UserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = new User
            {
                Username = request.Username,
                Password = Utils.HashPassword(request.Password),
                RoleId = request.RoleId
            };

            var isUserAdded = _userRepository.AddUser(user);

            if (isUserAdded)
            {
                return Ok(new { message = "User added successfully." });
            }

            return StatusCode(500, new { message = "An error occurred while adding the user." });
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var result = _userRepository.DeleteUser(id);
                if (result)
                    return Ok("User data deleted successfully.");
                return BadRequest("Failed to delete User data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }


        [HttpGet("get/all")]
        public IActionResult GetUsers()
        {
            try
            {
                var Users = _userRepository.GetUsers();
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

    }

    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
