using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SperanzaPizzaApi.Data.DTO.UserService;
using SperanzaPizzaApi.Services.Users;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<CreateUserResponse>> PostDmUsersRegister([FromBody]CreateUserRequest parameters)
        {
            return await _userService.RegisterUser(parameters);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<LoginUserResponse>> PostDmUsersLogin([FromBody]LoginUserRequest parameters)
        {
            return await _userService.LoginUser(parameters);
        }
    }
}