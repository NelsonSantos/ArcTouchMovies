using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArcTouchMovies.Views
{
    public class BasePage : ContentPage
    {
        private double _width;
        private double _height;
        private DisplayOrientation m_Orientation;

        public event EventHandler<PageOrientationEventArgs> OnOrientationChanged = (e, a) => { };

        public BasePage()
            : base()
        {
            Init();
            this.Orientation = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation;
        }

        public DisplayOrientation Orientation
        {
            get { return m_Orientation; }
            private set
            {
                m_Orientation = value; 
                this.OnOrientationChanged?.Invoke(this, new PageOrientationEventArgs(value));
            }
        }

        private void Init()
        {
            _width = this.Width;
            _height = this.Height;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;
            const double sizenotallocated = -1;

            base.OnSizeAllocated(width, height);
            if (Equals(_width, width) && Equals(_height, height)) return;

            _width = width;
            _height = height;

            // ignore if the previous height was size unallocated
            if (Equals(oldWidth, sizenotallocated)) return;

            // Has the device been rotated ?
            if (!Equals(width, oldWidth))
            {
                var _orientation= (width < height) ? DisplayOrientation.Portrait : DisplayOrientation.Landscape;
                this.Orientation = _orientation;
            }
        }
    }
    public class PageOrientationEventArgs : EventArgs
    {
        public PageOrientationEventArgs(DisplayOrientation orientation)
        {
            Orientation = orientation;
        }

        public DisplayOrientation Orientation { get; }
    }
}
