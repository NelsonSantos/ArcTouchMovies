using System.Net;

namespace ArcTouchMovies.ApiAccess
{
    public class ServiceStatus
    {
        public ServiceStatus(bool success, HttpStatusCode statusCode, string statusMessage = "")
        {
            Success = success;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }
        public bool Success { get; }
        public string StatusMessage { get; }
        public HttpStatusCode StatusCode { get; }
    }
}