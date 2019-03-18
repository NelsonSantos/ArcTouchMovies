using System.Net;

namespace ArcTouchMovies.ApiAccess
{
    public class ServiceResult<T> : ServiceStatus
    {
        public T Result { get; }
        public ServiceResult(T result, bool success, HttpStatusCode statusCode, string statusMessage = "") : base(success, statusCode, statusMessage)
        {
            Result = result;
        }
    }
}
