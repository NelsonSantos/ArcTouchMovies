using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArcTouchMovies.Models;
using ArcTouchMovies.Views;
using ArcTouchMovies.ViewModels;

namespace ArcTouchMovies.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationPage.BarBackgroundColor = (Color)App.Current.Resources["NavigationPrimary"];
        }
    }
}