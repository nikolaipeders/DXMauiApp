using DevExpress.Maui.DataForm;
using DXMauiApp.Models;

namespace DXMauiApp.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        public const string ViewName = "NewItemPage";


        string name;
        public string Name
        {
            get => this.name;
            set => SetProperty(ref this.name, value);
        }

        string location;
        public string Location
        {
            get => this.location;
            set => SetProperty(ref this.location, value);
        }

        string serial;
        public string Serial
        {
            get => this.serial;
            set => SetProperty(ref this.serial, value);
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

        TokenRequest token;
        public TokenRequest Token
        {
            get => this.token;
            set => SetProperty(ref this.token, value);
        }
        string id = string.Empty;
        public string Id
        {
            get => this.id;
            set => SetProperty(ref this.id, value);
        }

        public NewItemViewModel()
        {
            Token = new TokenRequest();
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
        }

        public async void OnAppearing()
        {
            await GetDetails();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }


        async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Navigation.GoBackAsync();
        }

        async void OnSave()
        {
            Lock newLock = new Lock();

            newLock.name = Name;
            newLock.location = Location;
            newLock.serial = Serial;
            

            var response = await LockService.SaveLockAsync(Token, newLock, true);

            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    ImageUrl = "checked.png";
                    ImageDescription = "Succesfully added lock!";
                }
            }
            else
            {
                ImageUrl = "error.png";
                ImageDescription = "Error!";
            }

            // Show status of call
            IsResultPopOpen = true;

            // Return
            await Task.Delay(1500);

            if (response.IsSuccessStatusCode)
            {
                ResetState();

                await Navigation.GoBackAsync();
            }
        }
        public void ResetState()
        {
            IsResultPopOpen = false;
            ButtonState = true;
        }

        public async Task GetDetails()
        {
            var authToken = await SecureStorage.Default.GetAsync("auth_token");
            Token.Token = authToken.Replace("\"", "");

            Id = await SecureStorage.Default.GetAsync("user_id");
        }
    }
}