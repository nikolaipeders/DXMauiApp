using DXMauiApp.Models;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DXMauiApp.ViewModels
{
    public class UnlockViewModel : BaseViewModel
    {
        string imageUrl;
        string imageDescription;
        bool buttonState;

        public string ImageUrl
        {
            get => this.imageUrl;
            set => SetProperty(ref this.imageUrl, value);
        }
        public string ImageDescription
        {
            get => this.imageDescription;
            set => SetProperty(ref this.imageDescription, value);
        }       
        public bool ButtonState
        {
            get => this.buttonState;
            set => SetProperty(ref this.buttonState, value);
        }

        public UnlockViewModel()
        {
            Title = "Unlock";
            ImageUrl = "smartdoor.png";
            ImageDescription = "Point phone towards reader and press button to unlock";
            ButtonState = true;
            UnlockCommand = new Command(async () => UnlockDoor());
        }

        async public void OnAppearing()
        {
            RedirectToLogin();
        }

        public async void UnlockDoor()
        {
            AudioManager am = new AudioManager();
            // DO MAGIC



            // IF SUCCESS
            ImageUrl = "door.png";
            ImageDescription = "Door unlocked";
            ButtonState = false;

            var audioSuccess = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alert.mp3"));
            audioSuccess.Play();

            // ELSE 
            //ImageUrl = "error.png";
            //ImageDescription = "Access denied";
            //ButtonState = false;
            //var audioError = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("wrong.mp3"));
            //audioError.Play();

            // RETURN TO ORIGINAL STATE
            await Task.Delay(2000);
            ImageUrl = "smartdoor.png";
            ImageDescription = "Point phone towards reader and press button to unlock";
            ButtonState = true;


        }

        public ICommand UnlockCommand { get; }
    }
}