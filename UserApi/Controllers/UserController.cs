using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();

            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserCreateDto user)
        {
            _userService.CreateUser(user);
            return Ok();
        }

        [HttpPut("EditUser/{id}")]
        public IActionResult EditUser([FromBody] UserCreateDto user, int id)
        {
            _userService.UpdateUser(user,id);
            return Ok();
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId) 
        {
            _userService.DeleteUser(userId);
            return Ok();
        }

    }
}
