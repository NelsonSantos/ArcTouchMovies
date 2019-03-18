using System;
using ArcTouchMovies.Models;

namespace ArcTouchMovies.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Movie SelectedMovie { get; }
        public ItemDetailViewModel(Movie movie)
        {
            this.Title = "Movie Details";
            this.SelectedMovie = movie;
        }
    }
}
