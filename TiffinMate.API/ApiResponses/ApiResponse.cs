using Supabase.Gotrue;
using System.Net;

namespace TiffinMate.API.ApiRespons
{
    public class ApiResponse<T>
    {
        public string status {  get; set; }
        public string message { get; set; }
       
        public T result { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string error_message { get; set; }
        public ApiResponse(string Status, string Message, T Result,HttpStatusCode StatusCode,string Errormesage)
        {
            status = Status;
            message = Message;
            statusCode = StatusCode;
            error_message = Errormesage;
            result = Result;
            statusCode = StatusCode;
            error_message = Errormesage;

        }


    }
}
