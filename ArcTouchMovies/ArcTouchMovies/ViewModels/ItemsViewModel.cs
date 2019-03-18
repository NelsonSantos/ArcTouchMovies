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
        public ICommand SearchItemsCommand { get; }
        public ICommand MovieSelectedCommand { get; }
        public ICommand OpenSearchCommand { get; }

        public ItemsViewModel(bool isSearch = false)
        {
            Title = "ArcTouch Movies";
            Movies = new ObservableCollection<Movie>();
            this.IsSearch = isSearch;

            this.SearchItemsCommand = new Command(async () => await this.SearchItems());
            this.MovieSelectedCommand = new Command<Movie>(async (movie) => await this.MovieSelected(movie));

            this.OpenSearchCommand = new Command(async () => await this.OpenSearch());

            this.LoadItemsCommand = new Command(async () => await LoadItems());
            this.LoadItemsCommand.Execute(null);
        }

        private async Task SearchItems()
        {
            this.Page = 1;
            this.TotalPages = 1;
            this.Movies.Clear();
            await this.LoadItems();
        }

        private async Task OpenSearch()
        {
            await this.NavigationService.NavigateToMovieSearch();
        }

        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public bool IsSearch { get; } = false;
        public string SearchKey { get; set; }

        private async Task LoadItems()
        {
            if (this.Page > this.TotalPages) return;

            MovieApiListResponse<Movie> _result = null;

            if (this.IsSearch)
            {
                if (string.IsNullOrEmpty(this.SearchKey)) return;

                var _searchKey = Uri.EscapeUriString(this.SearchKey);
                _result = await this.Get<MovieApiListResponse<Movie>>(BaseUrl, $"search/movie?query={_searchKey}&api_key={ApiKey}&language=en-US&page={this.Page}");
            }
            else
            {
                _result = await this.Get<MovieApiListResponse<Movie>>(BaseUrl, $"movie/upcoming?api_key={ApiKey}&language=en-US&page={this.Page}");
            }

            this.TotalPages = _result.TotalPages;
            this.Page++;

            foreach (var _movie in _result.Results)
            {
                this.Movies.Add(_movie);
            }
        }

        private async Task MovieSelected(Movie movie)
        {
            await this.NavigationService.NavigateToMovieDetails(movie);
        }
    }
}