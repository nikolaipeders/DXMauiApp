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

        public Command RegisterCommand { get; }


        async void OnRegisterClicked()
        {
            // TO DO CALL REGISTER SERVICE 

            // IF SUCCESSFUL CALL

            // TO DO SAVE TOKEN IN REALM IO 

            // THEN GO BACK
            await Navigation.GoBackAsync();
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