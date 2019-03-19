using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ArcTouchMovies.ApiAccess;
using Xamarin.Forms;

using ArcTouchMovies.Models;
using ArcTouchMovies.Services;

namespace ArcTouchMovies.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public const int DefaultRetryCount = 3;
        public const int DefaultApiTimeoutSeconds = 15;
        public const string BaseUrl = "https://api.themoviedb.org/3";
        public const string ApiKey = "1f54bd990f1cdfb230adb312546d765d";
        public bool IsBusy { get; set; }
        public string Title { get; set; }
        public INavigationService NavigationService { get; } = Xamarin.Forms.DependencyService.Get<INavigationService>();
        protected BaseViewModel()
        {
            if (m_BasicDataStore.GetConfiguration() == null)
            {
                Task.Run(async () => await this.InitConfigurationFunctions()).Wait();
            }
        }

        private async Task InitConfigurationFunctions()
        {
            var _conf = await this.Get<Configuration>(BaseUrl, $"configuration?api_key={ApiKey}");
            m_BasicDataStore.SetConfiguration(_conf);

            var _genres = await this.Get<GenreList>(BaseUrl, $"genre/movie/list?api_key={ApiKey}");
            m_BasicDataStore.SetGenres(_genres.Genres);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public FlurlCalls Api { get; private set; }
        public void SetApiObject(string baseUrl)
        {
            this.Api = new ApiAccess.FlurlCalls(baseUrl);
        }
        private async Task InitializeApiObject(string baseUrl, bool useAuthentication = true)
        {
            this.SetApiObject(baseUrl);
            //if (useAuthentication)
            //{
            //    if (string.IsNullOrEmpty(this.Token))
            //    {
            //        await this.GetNewToken(this.Usuario.Email, this.Usuario.Senha);
            //        this.SetApiObject(baseUrl);
            //    }

            //    if (this.TokenTimeout < DateTime.Now)
            //    {
            //        await this.GetNewToken(this.Usuario.Email, this.Usuario.Senha);
            //        this.SetApiObject(baseUrl);
            //    }
            //    this.Api.ClearHeaderList();
            //    this.Api.AddHeader("Authorization", $"Bearer {this.Token}");
            //}
        }
        private readonly IBasicDataStore m_BasicDataStore = Xamarin.Forms.DependencyService.Get<IBasicDataStore>();
        public Configuration Configuration => m_BasicDataStore.GetConfiguration();
        public IEnumerable<Genre> Genres => m_BasicDataStore.GetGenres();

        public async Task<TResult> Get<TResult>(string baseUrl, string methodName, Action<ServiceResult<TResult>> errorAction = null, bool useAuthentication = true, int retryCount = DefaultRetryCount, int secondsTimeout = DefaultApiTimeoutSeconds, bool showLoading = false, [CallerMemberName] string callerMemberName = null)
        {
            await this.InitializeApiObject(baseUrl, useAuthentication);

            var _result = await Api.GetAsync<TResult>(methodName, retryCount, secondsTimeout, callerMemberName: callerMemberName);

            if (_result.Success)
            {
                var _ret = _result.Result;
                return _ret;
            }
            else
            {
                await this.ResolveResult(_result, errorAction);
            }
            return default(TResult);
        }
        private async Task ResolveResult<TResult>(ServiceResult<TResult> result, Action<ServiceResult<TResult>> action)
        {
            if (action != null)
            {
                await Task.Run(() => action.Invoke(result));
            }
        }

    }
}
