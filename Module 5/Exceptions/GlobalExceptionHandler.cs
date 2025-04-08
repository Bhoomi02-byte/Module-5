using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Module_5.Exceptions;

namespace Module_5.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            
            var requestInfo = await RequestInfo.CreateAsync(httpContext);

            
            var responseInfo = new ResponseInfo(exception);
            httpContext.Response.StatusCode = responseInfo.StatusCode;
            httpContext.Response.ContentType = "application/json";

            string jsonResponse = await responseInfo.SerializeAsync(httpContext);
            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

            stopwatch.Stop();
            double executionTime = stopwatch.Elapsed.TotalSeconds;

           
            Log.Error("Timestamp: {Timestamp}\n" +
                      "IP Address: {IpAddress}\n" +
                      "Request URL: {RequestUrl}\n" +
                      "Method: {Method}\n" +
                      "User: {User}\n" +
                      "Request Headers: {RequestHeaders}\n" +
                      "Request Body: {RequestBody}\n" +
                      "Response Headers: {ResponseHeaders}\n" +
                      "Status Code: {StatusCode}\n" +
                      "Execution Time: {ExecutionTime} seconds\n",
                requestInfo.Timestamp, requestInfo.IpAddress, requestInfo.RequestUrl,
                requestInfo.Method, requestInfo.User, requestInfo.RequestHeaders,
                requestInfo.RequestBody, responseInfo.ResponseHeaders,
                responseInfo.StatusCode, executionTime
            );

            return true;
        }
    }
}
