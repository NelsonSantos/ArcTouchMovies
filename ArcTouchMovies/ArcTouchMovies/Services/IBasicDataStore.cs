using System.Collections.Generic;
using ArcTouchMovies.Models;

namespace ArcTouchMovies.Services
{
    public interface IBasicDataStore
    {
        void SetConfiguration(Configuration configuration);
        Configuration GetConfiguration();
        void SetGenres(IEnumerable<Genre> genres);
        IEnumerable<Genre> GetGenres();
    }
}