namespace Remore.Backend.Models
{
    public class ResponseEntity
    {      
        public bool Ok { get; init; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
    }

    public static class Extensions
    {
        public static ResponseEntity AsOkResponse(this object data)
        {
            return new ResponseEntity
            {
                Ok = true,
                Data = data,
                Errors = null
            };
        }
        public static ResponseEntity AsErrorResponse(this List<string> errors)
        {
            return new ResponseEntity
            {
                Ok = false,
                Data = null,
                Errors = errors
            };
        }
        public static ResponseEntity AsErrorResponse(this string error)
        {
            return new ResponseEntity
            {
                Ok = false,
                Data = null,
                Errors = new List<string> { error }
            };
        }
        public static ResponseEntity AsErrorResponse(this string[] errors)
        {
            return new ResponseEntity
            {
                Ok = false,
                Data = null,
                Errors = errors.ToList()
            };
        }
    }
}
