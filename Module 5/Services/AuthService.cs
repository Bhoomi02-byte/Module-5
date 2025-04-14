using MongoDB.Driver;
using Module_5.DTO;
using Module_5.Collections;
using Module_5.Utilities;
using Module_5.Data;

namespace Module_5.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoDbContext _db;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly EmailService _emailService;

        public AuthService(IMongoDbContext db, JwtTokenHelper jwtTokenHelper, EmailService emailService)
        {
            _db = db;
            _jwtTokenHelper = jwtTokenHelper;
            _emailService = emailService;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _db.Users.Find(u => u.Email == registerDto.Email).AnyAsync();

            if (existingUser)
                return false;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                HashPassword = hashedPassword,
                UserRole = registerDto.UserRole
            };

            await _db.Users.InsertOneAsync(newUser);

            string subject = "Welcome to Our Blog!";
            string body = EmailTemplates.WelcomeEmail(registerDto.Name, registerDto.UserRole);

            bool emailSent = await _emailService.SendEmailAsync(registerDto.Email, subject, body);

            if (!emailSent)
            {
                Console.WriteLine(JsonHelper.GetMessage(129));
            }

            return true;
        }

        public async Task<object> LoginAsync(LoginDto loginDto)
        {
            
            var user = await _db.Users.Find(u => u.Email == loginDto.Email).FirstOrDefaultAsync();

            if (user == null || !VerifyPassword(loginDto.Password, user.HashPassword))
                return null;

            string token = _jwtTokenHelper.GenerateJwtToken(user.Id, user.Name, user.Email, user.UserRole,user.TokenVersion);
            

            return new
            {
                name = user.Name,
                email = user.Email,
                Role = user.UserRole.ToString(),
                auth_Token = token
            };
        }

        public async Task<string> LogoutAsync(string userId) {

            var update = Builders<User>.Update.Inc(u => u.TokenVersion, 1);

            var result = await _db.Users.UpdateOneAsync(
                u => u.Id == userId,
                update
            );

            if (result.ModifiedCount == 1)
            {
                return JsonHelper.GetMessage(156);
            }

            return JsonHelper.GetMessage(157);
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _db.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        }

        private bool VerifyPassword(string enteredPassword, string storedHashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashPassword);
        }
    }
}
