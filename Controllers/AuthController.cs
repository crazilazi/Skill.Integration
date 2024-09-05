using Microsoft.AspNetCore.Mvc;
using Skill.Integration.Models;
using Skill.Integration.Services;

namespace Skill.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="model">The registration details.</param>
        /// <returns>A JWT token for the newly registered user.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            try
            {
                var token = await _authService.RegisterAsync(register.Username, register.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>A JWT token for the authenticated user.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var token = await _authService.LoginAsync(login.Username, login.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
