using DXMauiApp.Models;
using Microsoft.Maui.Controls;
using System;

namespace DXMauiApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string mail;
        public string Mail
        {
            get => this.mail;
            set => SetProperty(ref this.mail, value);
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
            // Subscribe to the "AccountRegistered" message
            MessagingCenter.Subscribe<RegisterViewModel, (string, string)>(this, "AccountRegistered", OnAccountRegistered);

            LoginCommand = new Command(OnLoginClicked);

            OpenRegisterPageCommand = new Command(async () => await Navigation.NavigateToAsync<RegisterViewModel>(false));
        }

        public void OnAppearing()
        {
            ResetState();
        }

        private void OnAccountRegistered(RegisterViewModel sender, (string, string) accountInfo)
        {
            // Update email and password fields
            Mail = accountInfo.Item1;
            Password = accountInfo.Item2;
        }

        async void OnLoginClicked()
        {
            User user = new User();
            user.email = Mail;
            user.password = Password;

            var result = await UserService.UserLoginAsync(user);

            if (result != null)
            {
                await SecureStorage.Default.SetAsync("auth_token", result.token);
                await SecureStorage.Default.SetAsync("user_id", result._id);

                ImageUrl = "checked.png";
                ImageDescription = "Succesfully logged in!";

                IsResultPopOpen = true;
                await Task.Delay(1500);
                IsResultPopOpen = false;
                
                await Task.Delay(500);
                await Navigation.GoBackAsync();
                await Navigation.NavigateToAsync<LocksViewModel>(true);
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
    }
}