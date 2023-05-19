using DevExpress.Data.Mask.Internal;
using DXMauiApp.Models;
using Microsoft.Maui.Controls;
using System;
using System.Security;

namespace DXMauiApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        string userName;
        string mail;
        string password;
        string confirmPassword;
        string rfid;

        public AccountViewModel()
        {
            Title = "My account";

            UpdateCommand = new Command(OnUpdateClicked, ValidateLogin);
            PropertyChanged +=
                (_, __) => UpdateCommand.ChangeCanExecute();

            SignOutCommand = new Command(OnSignOutClicked);
            PropertyChanged +=
            (_, __) => SignOutCommand.ChangeCanExecute();

        }

        public void OnAppearing()
        {
            RedirectToLogin();

            // TO DO GET RFID OF PHONE

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

        public Command UpdateCommand { get; }
        public Command SignOutCommand { get; }


        async void OnUpdateClicked()
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

        async void OnSignOutClicked()
        {
            TokenRequest request = new TokenRequest();

            var authToken = await SecureStorage.Default.GetAsync("auth_token");

            request.Token = authToken.Replace("\"", "");

            var result = await UserService.UserSignOutAsync(request);

            if (result.IsSuccessStatusCode)
            {
                await SecureStorage.Default.SetAsync("auth_token", "");

                RedirectToLogin();
            }
        }
    }
}