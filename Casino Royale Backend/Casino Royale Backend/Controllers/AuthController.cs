using Casino_Royale_Backend.Interfaces;
using Casino_Royale_Backend.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Casino_Royale_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController (IAuthService authService) : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await authService.RegisterUserAsync(request);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await authService.LoginUserAsync(request);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
