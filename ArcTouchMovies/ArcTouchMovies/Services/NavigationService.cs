using System.Threading.Tasks;
using ArcTouchMovies.Models;
using ArcTouchMovies.Views;

namespace ArcTouchMovies.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateBack()
        {
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
        }

        public async Task NavigateToMovieDetails(Movie movie)
        {
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(movie)));
        }
    }
}