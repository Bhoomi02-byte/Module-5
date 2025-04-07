using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Module_5.Exceptions;
using Module_5.Utilities; 

namespace Module_5.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = exception switch
            {
                GlobalException => 400,
                UnauthorizedAccessException => 401,
                KeyNotFoundException => 404,
                _ => 500
            };

            var apiResponse = new ApiResponse( false,statusCode,exception is GlobalException? exception.Message: JsonHelper.GetMessage(127), null);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            string json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json, cancellationToken);

            return true;
        }
    }
}
