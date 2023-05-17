using Microsoft.Maui.Controls;
using System;

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
        }

        public void OnAppearing()
        {
            RedirectToLogin();

            // TO DO GET RFID 

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
    }
}