using BBL.DTOModels;
using BBL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO userDto)
        {
            var isAuthenticated = _userService.Login(userDto);
            if (isAuthenticated)
            {
                return Ok(new { Message = "Login successful" });
            }
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        // POST: api/users/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return Ok(new { Message = "Logout successful" });
        }
    }
}
