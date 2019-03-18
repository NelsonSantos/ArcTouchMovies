using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArcTouchMovies.CustomViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MovieDetailsHorizontal : ContentView
	{
		public MovieDetailsHorizontal ()
		{
			InitializeComponent ();
		}
	}
}