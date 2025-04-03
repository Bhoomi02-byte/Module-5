using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.DTO;
using Module_5.Models.Entities;
using Module_5.Utilities;

namespace Module_5.Services

{
    public class AuthService:IAuthService
    {
        private readonly BlogDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly EmailService _emailService;

        public AuthService(BlogDbContext context, JwtTokenHelper jwtTokenHelper, EmailService emailService)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _emailService = emailService;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            bool existingUser = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);

            if (existingUser)
                return false;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                HashPassword = hashedPassword,
                UserRole = registerDto.UserRole,
               
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            string subject = "Welcome to Our Blog!";
            string body = EmailTemplates.WelcomeEmail(registerDto.Name, registerDto.UserRole);

            bool emailSent = await _emailService.SendEmailAsync(registerDto.Email, subject, body);

            if (!emailSent)
            {
                Console.WriteLine("Email failed to send!");
            }


            return true;
}

        public async Task<object> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Password, user.HashPassword))
                return null;

            string token = _jwtTokenHelper.GenerateJwtToken(user.Id, user.Name, user.Email, user.UserRole);

            return new 
            {
                 name=user.Name,
                 email=user.Email,
                 Role=user.UserRole.ToString(),
                 auth_Token = token
            };
        }

        private bool VerifyPassword(string enteredPassword, string storedHashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashPassword);
        }
        
       

      
    }
}
