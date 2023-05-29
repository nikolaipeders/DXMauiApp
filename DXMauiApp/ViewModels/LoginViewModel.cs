using DXMauiApp.Models;
using Microsoft.Maui.Controls;
using System;

namespace DXMauiApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string userName;
        string password;
        bool isPopOpen = false;

        public LoginViewModel()
        {
            Title = "Login";

            LoginCommand = new Command(OnLoginClicked, ValidateLogin);

            OpenRegisterPageCommand = new Command(async () => await Navigation.NavigateToAsync<RegisterViewModel>(false));

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

        public bool IsPopOpen
        {
            get => this.isPopOpen;
            set => SetProperty(ref this.isPopOpen, value);
        }

        public Command LoginCommand { get; }
        public Command OpenRegisterPageCommand { get; }


        async void OnLoginClicked()
        {
            User user = new User();

            user.Email = UserName;
            user.Password = Password;

            var result = await UserService.UserLoginAsync(user);

            if (result != null)
            {
                await SecureStorage.Default.SetAsync("auth_token", result);

                IsPopOpen = true;
                await Task.Delay(1500);
                IsPopOpen = false;
                await Task.Delay(500);

                await Navigation.NavigateToAsync<ItemsViewModel>(true);
            }
        }

        bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(UserName)
                && !String.IsNullOrWhiteSpace(Password);
        }
    }
}