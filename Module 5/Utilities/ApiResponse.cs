
namespace Module_5.Utilities
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse() { }
        public ApiResponse(bool success, int statusCode, string message, object data)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data ?? new { };
        }


    }
}
