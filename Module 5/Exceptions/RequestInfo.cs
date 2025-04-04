using System.Text.Json;

namespace Module_5.Exceptions
{
    public class RequestInfo
    {
        public string Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string RequestUrl { get; set; }
        public string Method { get; set; }
        public string User { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public static async Task<RequestInfo> CreateAsync(HttpContext context)
        {
            return new RequestInfo
            {
                Timestamp = DateTime.UtcNow.ToString("HH:mm:ss tt (UTC)"),
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                RequestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                Method = context.Request.Method,
                User = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous User (Not Logged In)",
                RequestHeaders = JsonSerializer.Serialize(context.Request.Headers),
                RequestBody = await GetRequestBodyAsync(context)
            };
        }
        private static async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                using var reader = new System.IO.StreamReader(context.Request.Body, leaveOpen: true);
                string body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; 
                return body;
            }
            return "None";
        }
    }
}
