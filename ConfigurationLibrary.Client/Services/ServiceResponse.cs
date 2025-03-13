using System.Net;

namespace ConfigurationLibrary.UI.Services
{
    public class ServiceResponse
    {
        public bool IsSuccess { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsFailed => !IsSuccess;
        public string Message { get; set; } = string.Empty;
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }
    }
}
