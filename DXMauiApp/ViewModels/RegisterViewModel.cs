using DXMauiApp.Models;
using DXMauiApp.Services;
using Microsoft.Maui.Controls;
using System;

namespace DXMauiApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        string name;
        public string Name
        {
            get => this.name;
            set => SetProperty(ref this.name, value);
        }

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

        string confirmPassword;
        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set => SetProperty(ref this.confirmPassword, value);
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

        public Command RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
        }

        public void OnAppearing()
        {
            ResetState();
        }

        async void OnRegisterClicked()
        {
            User user = new User();

            user.name = Name;
            user.email = Mail;
            user.password = Password;

            var response = await UserService.CreateUserAsync(user);

            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    ImageUrl = "checked.png";
                    ImageDescription = "Succesfully registered account!";
                }
            }
            else
            {
                ImageUrl = "error.png";
                ImageDescription = "Register failed!";
            }

            // Show status of call
            IsResultPopOpen = true;

            // Return
            await Task.Delay(1500);
            
            if (response.IsSuccessStatusCode)
            {
                // Publish message with email and password
                MessagingCenter.Send(this, "AccountRegistered", (Mail, Password));

                ResetState();

                await Navigation.GoBackAsync();
            }
        }

        public void ResetState()
        {
            Title = "Register";
            IsResultPopOpen = false;
            ButtonState = true;
        }
    }
}