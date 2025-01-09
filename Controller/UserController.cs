using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using UserRepository.Repositories;

namespace User.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository.Repositories.UserRepository _userRepository;

        public AuthController(UserRepository.Repositories.UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var isValidUser = _userRepository.ValidateUser(loginRequest.Username, loginRequest.Password);

            if (isValidUser)
            {
                return Ok(new { message = "Login successful." });
            }

            return Unauthorized(new { message = "Invalid username or password." });
        }

        [HttpPost("add")]
        public IActionResult AddUser([FromBody] UserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = new UserRepository.Repositories.User
            {
                Username = request.Username,
                Password = Utils.HashPassword(request.Password)
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
    }
}
