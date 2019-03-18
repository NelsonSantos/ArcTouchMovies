using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ArcTouchMovies.Models
{
    public class Movie : BaseModel
    {
        private string m_GenreList = null;

        [JsonConstructor]
        public Movie(string poster_path, string backdrop_path)
        {
            var _url = this.Configuration.Images.SecureBaseUrl;
            var _posterSize = this.Configuration.Images.PosterSizes[this.Configuration.Images.PosterSizes.Length - 1];
            var _backdropSize = this.Configuration.Images.BackDropSizes[this.Configuration.Images.BackDropSizes.Length - 1];

            this.Poster = $"{_url}/{_posterSize}/{poster_path}";
            this.Backdrop = $"{_url}/{_backdropSize}/{backdrop_path}";
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        public string Poster { get; set; }
        public string Backdrop { get; set; }

        public string GenreList
        {
            get
            {
                if (m_GenreList != null) return m_GenreList;

                foreach (var _genre in this.Genres.Where(g => this.GenreIds.Contains(g.Id)))
                {
                    m_GenreList += string.IsNullOrEmpty(m_GenreList) ? "" : ", ";
                    m_GenreList += _genre.Name;
                }

                return m_GenreList; 
            }
        }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }
    }
}