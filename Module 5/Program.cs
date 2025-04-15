using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Module_5.Services;
using Module_5.Utilities;
using Module_5.Exceptions;
using Microsoft.Extensions.FileProviders;
using Module_5.Middlware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Reflection;



namespace Module_5
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://0.0.0.0:5000");

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .WriteTo.File("Logs/api-log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Logs to file
             .CreateLogger();
            
            builder.Services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<JwtTokenHelper>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISubscribeService, SubscribeService>();



            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddSingleton<EmailService>();
            
            builder.Services.AddControllers();

            
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddProblemDetails();
            builder.Services.AddSingleton<GlobalExceptionHandler>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .Select(ms => new
                        {
                            Field = ms.Key,
                            Messages = ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        });

                    return new BadRequestObjectResult(new ApiResponse(false,400, JsonHelper.GetMessage(154),errors));
                };
            });

            var app = builder.Build();
            app.UseMiddleware<RequestResponseMiddleware>();
            app.UseStaticFiles();

            
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");


            app.MapControllers();

            app.Run();
        }
    }
}
