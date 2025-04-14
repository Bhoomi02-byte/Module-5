using Module_5.Collections;
using Module_5.DTO;
using Module_5.Utilities;

namespace Module_5.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<object> LoginAsync(LoginDto loginDto);

        Task<string> LogoutAsync(string userId);
        Task<User> GetUserByIdAsync(string userId);
    }
}
