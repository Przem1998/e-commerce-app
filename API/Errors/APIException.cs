namespace API.Errors
{
    public class APIException : ApiResponse
    {
        public APIException(int statusCode, string message = null, string details=null) : base(statusCode, message)
        {
            Details=details;
        }
        public string Details { get; set; }
    }
}