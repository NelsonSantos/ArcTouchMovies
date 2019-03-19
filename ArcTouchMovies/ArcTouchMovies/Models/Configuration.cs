using Newtonsoft.Json;

namespace ArcTouchMovies.Models
{
    public class Configuration
    {
        [JsonProperty("images")]
        public ImagesSettings Images { get; set; }

    }
    public class ImagesSettings
    {
        [JsonProperty("base_url")]
        public string BaseUrl { get; set; }

        [JsonProperty("secure_base_url")]
        public string SecureBaseUrl { get; set; }

        [JsonProperty("backdrop_sizes")]
        public string[] BackDropSizes { get; set; }

        [JsonProperty("poster_sizes")]
        public string[] PosterSizes { get; set; }
    }
}