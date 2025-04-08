using System.Text.Json;

namespace Module_5.Exceptions
{
        public class ResponseInfo
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string ResponseHeaders { get; set; }

            public ResponseInfo(Exception exception)
            {
                StatusCode = exception is GlobalException ? 400 : 500;
                Message = exception is GlobalException ? exception.Message : "An unexpected error occurred.";
                ResponseHeaders = "";
            }

            public async Task<string> SerializeAsync(HttpContext context)
            {
                ResponseHeaders = JsonSerializer.Serialize(context.Response.Headers);
                var responseObj = new
                {
                    success = false,
                    statusCode = StatusCode,
                    message = Message
                };
                return JsonSerializer.Serialize(responseObj);
            }
        
    }
}
