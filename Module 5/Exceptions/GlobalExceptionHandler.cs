using Microsoft.AspNetCore.Diagnostics;
using Module_5.Exceptions;
using Module_5.Utilities;
using System.Text.Json;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode;
        object responseData = null;
        string message;

        switch (exception)
        {
            case GlobalException globalException:
                statusCode = StatusCodes.Status400BadRequest;
                message = globalException.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                message = JsonHelper.GetMessage(127); 
                break;

            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = JsonHelper.GetMessage(127);
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = JsonHelper.GetMessage(127);
                break;
        }

        var apiResponse = new ApiResponse( false,statusCode, message, responseData);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(apiResponse);
        await context.Response.WriteAsync(json, cancellationToken);

        return true;
    }
}
