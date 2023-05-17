using Microsoft.Maui.Controls;
using System;

namespace DXMauiApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string userName;
        string password;

        public LoginViewModel()
        {
            Title = "Login";

            LoginCommand = new Command(OnLoginClicked, ValidateLogin);

            OpenRegisterPageCommand = new Command(async () => await Navigation.NavigateToAsync<RegisterViewModel>(null));

            PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }


        public string UserName
        {
            get => this.userName;
            set => SetProperty(ref this.userName, value);
        }

        public string Password
        {
            get => this.password;
            set => SetProperty(ref this.password, value);
        }

        public Command LoginCommand { get; }
        public Command OpenRegisterPageCommand { get; }


        async void OnLoginClicked()
        {
            //await SecureStorage.Default.SetAsync("auth_token", "secret-oauth-token-value");
            await Navigation.NavigateToAsync<UnlockViewModel>(true);
        }

        bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(UserName)
                && !String.IsNullOrWhiteSpace(Password);
        }
    }
}