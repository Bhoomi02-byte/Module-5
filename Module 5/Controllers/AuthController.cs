using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Registers a new user (author or user).
        /// </summary>
        /// <param name="registerDto">The user details including name, email, password, and role.</param>
        /// <returns>
        /// 201 Created with user info if successful, or 
        /// 400 Conflict if the user already exists.
        /// </returns>
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

        // <summary>
        /// Logs in a user by validating email and password credentials.
        /// </summary>
        /// <param name="loginDto">Login credentials (email and password).</param>
        /// <returns>
        /// 201 OK with auth token and user info if credentials are valid, 
        /// or 404 Not Found if login fails.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
           var response = await _authservice.LoginAsync(loginDto);

            if (response==null)
                return NotFound(new ApiResponse(false, 404,JsonHelper.GetMessage(102), null));

            return Ok(new ApiResponse(true, 201, JsonHelper.GetMessage(123), response));   
        }


        /// <summary>
        /// Logs out the currently authenticated user by invalidating the token version.
        /// </summary>
        /// <returns>
        /// 201 OK if logout is successful,
        /// or 400 Bad Request if the user is not found or already logged out.
        /// </returns>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var result = await _authservice.LogoutAsync(userId);
            if(result== JsonHelper.GetMessage(156))
            {
                return Ok(new ApiResponse(true, 201, result, null));
            }

            return Conflict(new ApiResponse(false, 400, result, null));
        }
    }
}
