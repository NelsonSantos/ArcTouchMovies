using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ArcTouchMovies.Services;

namespace ArcTouchMovies.Models
{
    public class BaseModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly IBasicDataStore m_BasicDataStore = Xamarin.Forms.DependencyService.Get<IBasicDataStore>();
        public Configuration Configuration => m_BasicDataStore.GetConfiguration();
        public IEnumerable<Genre> Genres => m_BasicDataStore.GetGenres();
    }
}