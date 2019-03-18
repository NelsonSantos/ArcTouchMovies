using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArcTouchMovies.Models
{
    public class Genre
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class GenreList
    {
        public IEnumerable<Genre> Genres { get; set; }
    }
}