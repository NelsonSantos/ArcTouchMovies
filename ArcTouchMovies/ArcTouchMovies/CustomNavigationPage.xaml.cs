using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArcTouchMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //public partial class CustomNavigationPage : ContentPage
    //{
    //    public CustomNavigationPage()
    //    {
    //        InitializeComponent();
    //    }
    //}
    public partial class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage() : base()
        {
            InitializeComponent();
        }

        public CustomNavigationPage(Page root) : base(root)
        {
            InitializeComponent();
            this.FirstPage = root;
        }

        public Page FirstPage { get; }

        public static readonly BindableProperty IsFirstPageProperty = BindableProperty.Create(
            nameof(CustomNavigationPage.IsFirstPage), 
            typeof(bool),
            typeof(CustomNavigationPage), 
            true, 
            BindingMode.OneWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = bindable as CustomNavigationPage;
                control.IsFirstPage = (bool)newValue;
            });

        public bool IsFirstPage
        {
            get => (bool)GetValue(IsFirstPageProperty);
            set => SetValue(IsFirstPageProperty, value);
        }
        public bool IgnoreLayoutChange { get; set; } = false;

        protected override void OnSizeAllocated(double width, double height)
        {
            if (!IgnoreLayoutChange)
                base.OnSizeAllocated(width, height);
        }
    }
}