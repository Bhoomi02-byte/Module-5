using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Module_5;
using Serilog;


public class GlobalExceptionHandler : IExceptionHandler
{
  

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew(); 

        
        string timestamp = DateTime.UtcNow.ToString("HH:mm:ss tt (UTC)");
        string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        string requestUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
        string method = httpContext.Request.Method;
        string user = httpContext.User.Identity?.IsAuthenticated == true ? httpContext.User.Identity.Name : JsonHelper.GetMessage(128);
        string requestHeaders = JsonSerializer.Serialize(httpContext.Request.Headers);
        string requestBody = httpContext.Request.ContentLength > 0 ? await new System.IO.StreamReader(httpContext.Request.Body).ReadToEndAsync() : "None";

        int statusCode = GetStatusCode(exception);
        var response = new
        {
            success = false, 
            statusCode = statusCode,
            message = exception is GlobalException ? exception.Message : JsonHelper.GetMessage(127)
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var jsonResponse = JsonSerializer.Serialize(response);
        await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

        string responseHeaders = JsonSerializer.Serialize(httpContext.Response.Headers);

        stopwatch.Stop(); 
        double executionTime = stopwatch.Elapsed.TotalSeconds;

        
        Log.Error("Time: {Timestamp} | IP: [{IPAddress}] | {RequestUrl} ({Method}) | USER: [{User}] | REQUEST_HEADERS: {RequestHeaders} | REQUEST BODY: {RequestBody} | RESPONSE_HEADERS: {ResponseHeaders}| STATUS_CODE ~ {StatusCode} | EXECUTION_TIME [{ExecutionTime} Seconds]",
            timestamp, ipAddress, requestUrl, method, user, requestHeaders, requestBody,responseHeaders, statusCode, executionTime);
        return true;
    }

    private int GetStatusCode(Exception ex)
    {
        return ex switch
        {
            GlobalException => 400, 
            _ => 500 
        };
    }
}
