using System.Net;

namespace BankAppWithAPI.Models
{
    /// <summary>
    /// A custom generic class that is sent as a response to the user.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}
