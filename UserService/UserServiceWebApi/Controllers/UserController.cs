using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models.User;
using UserServiceWebApi.Service;

namespace UserServiceWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UsersService _userService;
        public UserController(UsersService userService)
        {
            _userService = userService;
        }

        #region Create
        [HttpPost]
        [Route("create")]
        public IActionResult Create(string username)
        {
            if(string.IsNullOrEmpty(username))
                return NotFound();
            var result = CreateAsync(username)
                .GetAwaiter()
                .GetResult();
            return Ok(result);
        }
        private async Task<bool> CreateAsync(string username) => await _userService.CreateUserAsync(username);
        #endregion

        #region GetAll
        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            var result = GetAllAsync()
                .GetAwaiter()
                .GetResult();
            return Ok(result);
        }
        private async Task<IEnumerable<UserDto>> GetAllAsync() => await _userService.GetAllUsersAsync();
        #endregion
    }
}
