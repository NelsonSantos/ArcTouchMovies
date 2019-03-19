using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcTouchMovies;
using ArcTouchMovies.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationRenderer))]
namespace ArcTouchMovies.iOS
{
    public class CustomNavigationRenderer : NavigationRenderer
    {
        CustomNavigationPage CustomNavigationPage => Element as CustomNavigationPage;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (CustomNavigationPage.CurrentPage == CustomNavigationPage.FirstPage)
            {
                CustomNavigationPage.IsFirstPage = true;
                CustomNavigationPage.IgnoreLayoutChange = false;
                return;
            }

            CustomNavigationPage.IsFirstPage = false;

            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}