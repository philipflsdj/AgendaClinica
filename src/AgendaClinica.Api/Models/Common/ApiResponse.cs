namespace AgendaClinica.Api.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public static ApiResponse<T> Ok(T? data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Fail(IEnumerable<string> errors, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        public static ApiResponse<T> Fail(string error, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new[] { error }
            };
        }
    }
}
