using System.Net.Http;
using Flurl.Http.Configuration;

namespace ArcTouchMovies.ApiAccess
{
    public class DependencieHttpClient : DefaultHttpClientFactory
    {
        public override HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler);
        }
    }
}