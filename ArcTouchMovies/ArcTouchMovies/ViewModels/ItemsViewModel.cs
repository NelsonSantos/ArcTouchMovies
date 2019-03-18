using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using ArcTouchMovies.Models;
using ArcTouchMovies.Views;

namespace ArcTouchMovies.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();
        public ICommand LoadItemsCommand { get; }
        public ICommand MovieSelectedCommand { get; }

        public ItemsViewModel()
        {
            Title = "ArcTouch Movies";
            Movies = new ObservableCollection<Movie>();

            this.MovieSelectedCommand = new Command<Movie>(async (movie) => await this.MovieSelected(movie));

            this.LoadItemsCommand = new Command(async () => await LoadItems());
            this.LoadItemsCommand.Execute(null);
        }
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        private async Task LoadItems()
        {
            //if (IsBusy)
            //    return;

            //IsBusy = true;

            if (this.Page > this.TotalPages) return;

            var _result = await this.Get<MovieApiListResponse<Movie>>(BaseUrl, $"movie/upcoming?api_key={ApiKey}&language=en-US&page={this.Page}");

            this.TotalPages = _result.TotalPages;
            this.Page++;

            foreach (var _movie in _result.Results)
            {
                this.Movies.Add(_movie);
            }

            //IsBusy = false;
        }

        private async Task MovieSelected(Movie movie)
        {
            await this.NavigationService.NavigateToMovieDetails(movie);
        }
    }
}