using DXMauiApp.Models;
using DXMauiApp.Services;
using Microsoft.Maui.Controls;
using System;

namespace DXMauiApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        string userName;
        string mail;
        string password;
        string confirmPassword;
        string rfid;
        bool isPopOpen = false;

        public RegisterViewModel()
        {
            Title = "Register";

            RegisterCommand = new Command(OnRegisterClicked, ValidateLogin);
            PropertyChanged +=
                (_, __) => RegisterCommand.ChangeCanExecute();
        }

        public string UserName
        {
            get => this.userName;
            set => SetProperty(ref this.userName, value);
        }

        public string Mail
        {
            get => this.mail;
            set => SetProperty(ref this.mail, value);
        }

        public string Password
        {
            get => this.password;
            set => SetProperty(ref this.password, value);
        }

        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set => SetProperty(ref this.confirmPassword, value);
        }
        public string RFID
        {
            get => this.rfid;
            set => SetProperty(ref this.rfid, value);
        }
        public bool IsPopOpen
        {
            get => this.isPopOpen;
            set => SetProperty(ref this.isPopOpen, value);
        }

        public Command RegisterCommand { get; }

        async void OnRegisterClicked()
        {
            User user = new User();

            user.Email = Mail;
            user.Password = Password;

            var result = await UserService.SaveUserAsync(user, true);

            if (result.IsSuccessStatusCode)
            {
                IsPopOpen = true;
                await Task.Delay(1500);
                IsPopOpen = false;
                await Task.Delay(500);

                await Navigation.NavigateToAsync<LoginViewModel>(true);
            }
        }

        bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(UserName)
                && !String.IsNullOrWhiteSpace(Mail)
                && !String.IsNullOrWhiteSpace(Password)
                && !String.IsNullOrWhiteSpace(ConfirmPassword)
                && Password.Equals(ConfirmPassword);
        }
    }
}