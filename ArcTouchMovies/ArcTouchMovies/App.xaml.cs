using System;
using ArcTouchMovies.ApiAccess;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArcTouchMovies.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ArcTouchMovies
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Xamarin.Forms.DependencyService.Register<Services.NavigationService>();
            Xamarin.Forms.DependencyService.Register<Services.BasicDataStore>();

            NavigationPage = new CustomNavigationPage(new ItemsPage());
            this.MainPage = NavigationPage;
        }
        public static CustomNavigationPage NavigationPage { get; set; }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
