using KybInfrastructure.Demo.Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public List<User> GetAllUsers()
            => _userService.GetAllUsers();

        [HttpGet]
        public List<User> GetAdminUsers()
            => _userService.GetAdminUsers();

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            _userService.AddUser(user);
            return Ok();
        }
    }
}
