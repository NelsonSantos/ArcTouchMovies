using System.Collections.Generic;
using ArcTouchMovies.Models;

namespace ArcTouchMovies.Services
{
    public class BasicDataStore : IBasicDataStore
    {
        private static Configuration Configuration;
        private static IEnumerable<Genre> Genres;
        public void SetConfiguration(Configuration configuration)
        {
            Configuration = configuration;
        }

        public Configuration GetConfiguration()
        {
            return Configuration;
        }

        public void SetGenres(IEnumerable<Genre> genres)
        {
            Genres = genres;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return Genres;
        }
    }
}