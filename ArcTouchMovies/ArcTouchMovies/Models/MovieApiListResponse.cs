using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ArcTouchMovies.Models
{
    public class MovieApiListResponse<T>
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("results")]
        public IEnumerable<T> Results { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
