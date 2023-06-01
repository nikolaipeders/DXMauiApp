using DXMauiApp.Models;
using Microsoft.Maui.Controls;
using System;

namespace DXMauiApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string userName;
        public string UserName
        {
            get => this.userName;
            set => SetProperty(ref this.userName, value);
        }

        string password;
        public string Password
        {
            get => this.password;
            set => SetProperty(ref this.password, value);
        }

        bool buttonState;
        public bool ButtonState
        {
            get => this.buttonState;
            set => SetProperty(ref this.buttonState, value);
        }

        string imageUrl;
        public string ImageUrl
        {
            get => this.imageUrl;
            set => SetProperty(ref this.imageUrl, value);
        }

        string imageDescription;
        public string ImageDescription
        {
            get => this.imageDescription;
            set => SetProperty(ref this.imageDescription, value);
        }

        bool isResultPopOpen = false;
        public bool IsResultPopOpen
        {
            get => this.isResultPopOpen;
            set
            {
                SetProperty(ref this.isResultPopOpen, value);
            }
        }

        public Command LoginCommand { get; }
        public Command OpenRegisterPageCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);

            OpenRegisterPageCommand = new Command(async () => await Navigation.NavigateToAsync<RegisterViewModel>(false));

            PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }

        public void OnAppearing()
        {
            ResetState();
        }


        async void OnLoginClicked()
        {
            User user = new User();

            user.Email = UserName;
            user.Password = Password;

            var result = await UserService.UserLoginAsync(user);

            if (result != null)
            {
                await SecureStorage.Default.SetAsync("auth_token", result);

                ImageUrl = "checked.png";
                ImageDescription = "Succesfully logged in!";

                IsResultPopOpen = true;
                await Task.Delay(1500);
                IsResultPopOpen = false;
                
                await Task.Delay(500);
                await Navigation.NavigateToAsync<ItemsViewModel>(true);
            }
            else
            {
                ImageUrl = "error.png";
                ImageDescription = "Login failed!";

                IsResultPopOpen = true;
                await Task.Delay(1500);
                IsResultPopOpen = false;
            }
        }

        public void ResetState()
        {
            Title = "Login";
            IsResultPopOpen = false;
            ButtonState = true;
        }

        bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(UserName)
                && !String.IsNullOrWhiteSpace(Password);
        }
    }
}