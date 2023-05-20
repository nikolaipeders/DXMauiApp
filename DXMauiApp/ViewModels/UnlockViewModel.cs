using Camera.MAUI;
using DXMauiApp.Models;
using DXMauiApp.Services;
using Plugin.Maui.Audio;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace DXMauiApp.ViewModels
{
    public class UnlockViewModel : BaseViewModel
    {
        string imageUrl;
        string imageDescription;
        string instructions;
        bool buttonState;
        bool isError = false;
        bool isSuccess = false;
        bool isResultPopOpen = false;
        ImageSource snapShot;

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
        public string Instructions
        {
            get => this.instructions;
            set => SetProperty(ref this.instructions, value);
        }
        public bool ButtonState
        {
            get => this.buttonState;
            set => SetProperty(ref this.buttonState, value);
        }
        public ImageSource SnapShot
        {
            get => this.snapShot;
            set
            {
                SetProperty(ref this.snapShot, value);
            }
        }
        public bool IsError
        {
            get => this.isError;
            set
            {
                SetProperty(ref this.isError, value);
            }
        }
        public bool IsSuccess
        {
            get => this.isSuccess;
            set
            {
                SetProperty(ref this.isSuccess, value);
            }
        }

        public bool IsResultPopOpen
        {
            get => this.isResultPopOpen;
            set
            {
                SetProperty(ref this.isResultPopOpen, value);
            }
        }
        public Command TakeSnapshotCmd { get; set; }

        public UnlockViewModel()
        {
            Title = "Unlock";
            ButtonState = true;

            TakeSnapshotCmd = new Command(TakePhoto);
        }

        public void OnAppearing()
        {
            RedirectToLogin();

            ResetState();
        }

        public async Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource imageSource)
        {
            Stream stream = await ((StreamImageSource)imageSource).Stream(CancellationToken.None);
            byte[] bytesAvailable = new byte[stream.Length];
            await stream.ReadAsync(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
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
                    string base64Photo = Convert.ToBase64String(photoBytes);

                    Debug.WriteLine("BASE64: " + base64Photo);

                    UnlockDoor(base64Photo);
                }
            }
        }

        public async void UnlockDoor(string base64)
        {
            // Prepare sound effects
            AudioManager am = new AudioManager();

            // State handling
            ButtonState = false;
            Instructions = "Verifying face ...";

            // Do REST magic
            await Task.Delay(3000);

            // IF SUCCESS
            ImageUrl = "door.png";
            ImageDescription = "Door unlocked!";
            var audioSuccess = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alert.mp3"));
            audioSuccess.Play();

            // IF FAIL
            //ImageUrl = "error.png";
            //ImageDescription = "Access denied";
            //var audioError = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("wrong.mp3"));
            //audioError.Play();

            // Show status of call
            IsResultPopOpen = true;

            // Return
            await Task.Delay(3000);

            // Reset state
            ResetState();
        }

        public void ResetState()
        {
            IsResultPopOpen = false;
            ButtonState = true;

            Instructions = "Point phone towards your face and press button to unlock";
            SnapShot = "smartdoor.png";
        }
    }
}