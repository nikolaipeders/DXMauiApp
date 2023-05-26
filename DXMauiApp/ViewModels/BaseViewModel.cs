using DXMauiApp.Models;
using DXMauiApp.Services;
using DXMauiApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DXMauiApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        bool isBusy = false;
        string title = string.Empty;

        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        public INavigationService Navigation => DependencyService.Get<INavigationService>();

        public IUserRestService UserService => DependencyService.Get<IUserRestService>();

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { SetProperty(ref this.isBusy, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { SetProperty(ref this.title, value); }
        }

        string baseToken = string.Empty;
        public string BaseToken
        {
            get { return this.baseToken; }
            set { SetProperty(ref this.baseToken, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void RedirectToLogin()
        {
            BaseToken = await SecureStorage.Default.GetAsync("auth_token");

            System.Diagnostics.Debug.WriteLine("SECURE STORAGE TOKEN IS: " + BaseToken);

            if (BaseToken == null || BaseToken.Equals(""))
            {
                // No value is associated with the key "auth_token", so redirect to login
                await Navigation.NavigateToAsync<LoginViewModel>(false);
            }
            else
            {
                TokenRequest request = new TokenRequest();

                request.Token = BaseToken.Replace("\"", "");

                var result = await UserService.UserConfirmAccessAsync(request);

                if (!result.IsSuccessStatusCode)
                {
                    await SecureStorage.Default.SetAsync("auth_token", "");

                    await Navigation.NavigateToAsync<LoginViewModel>(false);

                }
            }
        }
    }
}