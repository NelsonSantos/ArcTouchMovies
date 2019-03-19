using System.Threading.Tasks;
using ArcTouchMovies.Models;

namespace ArcTouchMovies.Services
{
    public interface INavigationService
    {
        Task NavigateBack();
        Task NavigateToMovieDetails(Movie movie);
        Task NavigateToMovieSearch();
    }
}