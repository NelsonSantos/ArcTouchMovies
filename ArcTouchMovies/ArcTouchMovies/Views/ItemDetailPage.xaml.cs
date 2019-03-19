using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArcTouchMovies.Models;
using ArcTouchMovies.ViewModels;

namespace ArcTouchMovies.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : BasePage
    {
        private ItemDetailViewModel m_ViewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            m_ViewModel = viewModel;
            this.SetContent(this.Orientation);
            OnOrientationChanged += ItemDetailPage_OnOrientationChanged;
        }

        private void ItemDetailPage_OnOrientationChanged(object sender, PageOrientationEventArgs e)
        {
            this.SetContent(e.Orientation);
        }

        private void SetContent(Xamarin.Essentials.DisplayOrientation orientation)
        {
            if (orientation == Xamarin.Essentials.DisplayOrientation.Portrait)
                this.Content = new CustomViews.MovieDetailsVertical();
            else
                this.Content = new CustomViews.MovieDetailsHorizontal();

            this.BindingContext = m_ViewModel;
        }
    }
}