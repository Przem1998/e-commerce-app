using System.Collections.Generic;

namespace API.Errors
{
    public class APIValidationErrorResponse : ApiResponse
    {
        public APIValidationErrorResponse() : base(400)
        {

        }
        public IEnumerable<string> Errors { get; set; }
        
    }
}