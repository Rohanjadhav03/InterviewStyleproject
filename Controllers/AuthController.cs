using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeApi.Models.Dtos;
using PracticeApi.Services;

namespace PracticeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService Authservice;
        public AuthController(IAuthService _Auth)
        {
            Authservice = _Auth;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequeset request)
        {
            var result = await Authservice.RegisterAsync(request);
            return Ok(new {message="Registered Successfully..",affectdrows=result});
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await Authservice.LoginAsync(request);
            return Ok(response);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var response = await Authservice.RefreshTokenAsync(refreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            string username = User.Identity.Name;
            int userId = int.Parse(User.FindFirst("UserId")?.Value);

            return Ok(new
            {
                Message = "Profile fetched",
                UserId = userId,
                Username = username
            });
        }
    }
}
