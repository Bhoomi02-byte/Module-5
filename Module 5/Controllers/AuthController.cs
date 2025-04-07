using Azure;
using Microsoft.AspNetCore.Mvc;
using Module_5.DTO;
using Module_5.Services;
using Module_5.Utilities;

namespace Module_5.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;

        public AuthController(IAuthService authService)
        {
            _authservice = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var response = await _authservice.RegisterAsync(registerDto);
            if (!response)
            {
                return Conflict(new ApiResponse(false,400,JsonHelper.GetMessage(101),null));
            }
            
            return Ok(new ApiResponse(true,201, JsonHelper.GetMessage(122), new
            {
                registerDto.Name,
                registerDto.Email,
                Role = registerDto.UserRole.ToString()
            
            }));
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
           var response = await _authservice.LoginAsync(loginDto);

            if (response==null)
                return NotFound(new ApiResponse(false, 400,JsonHelper.GetMessage(102), null));

            return Ok(new ApiResponse(true, 201, JsonHelper.GetMessage(123), response));   
        }
    }
}
