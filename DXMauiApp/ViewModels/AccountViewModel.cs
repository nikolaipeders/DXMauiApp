using DevExpress.Data.Mask.Internal;
using DevExpress.Services.Implementation;
using DXMauiApp.Models;
using DXMauiApp.Services;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Security;

namespace DXMauiApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        string userName;
        public string UserName
        {
            get => this.userName;
            set => SetProperty(ref this.userName, value);
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

        string imageBase64;
        public string ImageBase64
        {
            get => this.imageBase64;
            set => SetProperty(ref this.imageBase64, value);
        }
        
        string instructions;
        public string Instructions
        {
            get => this.instructions;
            set => SetProperty(ref this.instructions, value);
        }
        
        bool buttonState;
        public bool ButtonState
        {
            get => this.buttonState;
            set => SetProperty(ref this.buttonState, value);
        }
        
        ImageSource snapShot;
        public ImageSource SnapShot
        {
            get => this.snapShot;
            set
            {
                SetProperty(ref this.snapShot, value);
            }
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

        public Command TakePictureCommand { get; }
        public Command UpdateCommand { get; }
        public Command SignOutCommand { get; }

        public AccountViewModel()
        {
            Title = "My account";

            ButtonState = true;

            GetDetails();

            UpdateCommand = new Command(OnUpdateClicked);

            TakePictureCommand = new Command(TakePhoto);
            
            SignOutCommand = new Command(OnSignOutClicked);

        }

        public void OnAppearing()
        {
            RedirectToLogin();

            GetDetails();
        }

        public async void TakePhoto()
        {
            if (MediaPicker.IsCaptureSupported)
            {

                FileResult photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    // Save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    localFileStream.Close();

                    SnapShot = ImageSource.FromFile(localFilePath);

                    // Convert photo file to byte array
                    byte[] photoBytes = File.ReadAllBytes(localFilePath);

                    // Encode the byte array as base64
                    ImageBase64 = Convert.ToBase64String(photoBytes);
                }
            }
        }

        async void OnUpdateClicked()
        {
            // Prepare sound effects
            AudioManager am = new AudioManager();

            // State handling
            ButtonState = false;

            // Do REST magic
            User user = new User();

            user.Email = Mail;
            user.Password = Password;
            if (ImageBase64 != null && ImageBase64.Length > 5)
            {
                user.Image = "data:image/jpeg;base64," + ImageBase64;
            }

            var result = await UserService.SaveUserAsync(user, false);

            if (result.IsSuccessStatusCode)
            {
                ImageUrl = "checked.png";
                ImageDescription = "Account updated!";
                var audioSuccess = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alert.mp3"));
                audioSuccess.Play();
            }

            else
            {
                ImageUrl = "error.png";
                ImageDescription = "Error";
                var audioError = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("wrong.mp3"));
                audioError.Play();
            }

            // Show status of call
            IsResultPopOpen = true;

            // Return
            await Task.Delay(1500);

            // Reset state
            ResetState();
        }

        public void ResetState()
        {
            IsResultPopOpen = false;
            ButtonState = true;
        }

        public async void GetDetails()
        {
            TokenRequest request = new TokenRequest();

            var authToken = await SecureStorage.Default.GetAsync("auth_token");

            request.Token = authToken.Replace("\"", "");

            User user = await UserService.GetUserByTokenAsync(request);

            if (user != null)
            {
                Mail = user.Email;

                if (user.Image != null)
                {
                    SnapShot = user.Image;
                }
                else
                {
                    SnapShot = "user.png";
                }
            }
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