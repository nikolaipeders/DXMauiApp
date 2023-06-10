using Com.Google.Android.Exoplayer2.Analytics;
using CommunityToolkit.Mvvm.Messaging;
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
        TokenRequest request;
        public TokenRequest Request
        {
            get => this.request;
            set => SetProperty(ref this.request, value);
        }

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

        string imageBase64 = string.Empty;
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

        bool isLoading = false;
        public bool IsLoading
        {
            get => this.isLoading;
            set
            {
                SetProperty(ref this.isLoading, value);
            }
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

        public Command NavigateToInvitesCmd { get; }
        public Command TakePictureCommand { get; }
        public Command UpdateCommand { get; }
        public AccountViewModel()
        {
            Title = "My account";

            Request = new TokenRequest();

            ButtonState = true;

            GetDetails();

            UpdateCommand = new Command(OnUpdateClicked);

            TakePictureCommand = new Command(TakePhoto);

            NavigateToInvitesCmd = new Command(() =>
            {
                Navigation.NavigateToAsync<InvitesViewModel>();
            });
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
            try
            {
                // Prepare sound effects
                AudioManager am = new AudioManager();

                // Show loading screen
                IsLoading = true;
                ImageUrl = "";
                ImageDescription = "Verifying...";
                IsResultPopOpen = true;

                User user = new User();

                user.name = Name;
                user.email = Mail;
                user.password = Password;
                if (ImageBase64 != null && ImageBase64.Length > 5)
                {
                    user.image = "data:image/jpeg;base64," + ImageBase64;
                }

                var result = await UserService.UpdateUserAsync(Request, user);

                IsLoading = false;

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ResetState()
        {
            IsResultPopOpen = false;
            IsLoading = false;
            ButtonState = true;
        }

        public async void GetDetails()
        {
            try
            {
                var authToken = await SecureStorage.Default.GetAsync("auth_token");
                var user_id = await SecureStorage.Default.GetAsync("user_id");

                if (authToken != null && user_id != null)
                {
                    Request.Token = authToken.Replace("\"", "");

                }

                User user = await UserService.GetUserByIdAsync(Request, user_id);

                if (user != null)
                {
                    Name = user.name;
                    Mail = user.email;

                    if (user.image != null)
                    {
                        SnapShot = user.image;
                    }
                    else
                    {
                        SnapShot = "user.png";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}