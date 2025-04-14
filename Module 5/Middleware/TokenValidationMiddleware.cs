using Microsoft.AspNetCore.Http;
using Module_5.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module_5.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var tokenVersionClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "TokenVersion");
                if (tokenVersionClaim == null || !int.TryParse(tokenVersionClaim.Value, out var tokenVersion))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token is no longer valid (version mismatch).");
                    return;
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                    var user = await authService.GetUserByIdAsync(userId);

                    if (user == null || user.TokenVersion != tokenVersion)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token is no longer valid (version mismatch).");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
