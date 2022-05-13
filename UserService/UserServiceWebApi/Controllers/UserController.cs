using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserServiceWebApi.Models;
using UserServiceWebApi.Service;

namespace UserServiceWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        #region Create
        [HttpPost]
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
        public IActionResult GetAll()
        {
            var result = GetAllAsync()
                .GetAwaiter()
                .GetResult();
            return Ok(result);
        }
        private async Task<IEnumerable<User>> GetAllAsync() => await _userService.GetAllUsersAsync();
        #endregion
    }
}
