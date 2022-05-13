using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using UserServiceWebApi.Models;
using UserServiceWebApi.Service;

namespace UserServiceWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;

        public AuthController(ILogger<AuthController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate(User user)
        {
            if (!ModelState.IsValid)
                return Unauthorized();

            try
            {
                var token = _authService.Authenticate(user);
                if (token == null)
                    return Unauthorized();
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ControllerContext.ActionDescriptor.ControllerName}" +
                    $"{this.ControllerContext.ActionDescriptor.ActionName} was crash by next:\n{ex.Message}");
                return Unauthorized();
            }
        }
    }
}
