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
        bool isImagePopOpen = false;
        bool isResultPopOpen = false;
        ImageSource snapShot;
        ImageSource snapShotFromByte;
        CameraInfo camera = null;
        CameraView cameraView;
        ObservableCollection<CameraInfo> cameras = new();

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
        public ImageSource SnapShotFromByte
        {
            get => this.snapShotFromByte;
            set
            {
                SetProperty(ref this.snapShotFromByte, value);
            }
        }
        public CameraInfo CameraObj
        {
            get => camera;
            set
            {
                camera = value;
                OnPropertyChanged(nameof(CameraObj));
                CameraView.AutoStartPreview = false;
                OnPropertyChanged(nameof(CameraView.AutoStartPreview));
                CameraView.AutoStartPreview = true;
                OnPropertyChanged(nameof(CameraView.AutoStartPreview));
            }
        }
        public CameraView CameraView
        {
            get => cameraView;
            set
            {
                cameraView = value;
                SetProperty(ref cameraView, value);
            }
        }
        public ObservableCollection<CameraInfo> Cameras
        {
            get => cameras;
            set
            {
                cameras = value;
                OnPropertyChanged(nameof(Cameras));
            }
        }
        public int NumCameras
        {
            set
            {
                if (value > 0)
                    CameraObj = Cameras.Last(); // Last is selfie 
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

        public bool IsImagePopOpen
        {
            get => this.isImagePopOpen;
            set
            {
                SetProperty(ref this.isImagePopOpen, value);
                CameraView.AutoStartPreview = !isImagePopOpen;
                OnPropertyChanged(nameof(CameraView.AutoStartPreview));
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
            Instructions = "Point phone towards your face and press button to unlock";
            ButtonState = true;

            TakeSnapshotCmd = new Command(UnlockDoor);
        }

        public void OnAppearing()
        {
            RedirectToLogin();

            CameraView.AutoStartPreview = false;
            OnPropertyChanged(nameof(CameraView.AutoStartPreview));
            CameraView.AutoStartPreview = true;
            OnPropertyChanged(nameof(CameraView.AutoStartPreview));
        }

        public async Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource imageSource)
        {
            Stream stream = await ((StreamImageSource)imageSource).Stream(CancellationToken.None);
            byte[] bytesAvailable = new byte[stream.Length];
            await stream.ReadAsync(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
        }

        public async void UnlockDoor()
        {
            // Prepare sound effects
            AudioManager am = new AudioManager();

            // Save Image
            SnapShot = CameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);

            var byteResult = await ConvertImageSourceToBytesAsync(SnapShot);

            var base64String = Convert.ToBase64String(byteResult);

            SnapShotFromByte = ImageSource.FromStream(() => new MemoryStream(byteResult));

            Debug.WriteLine("BASE64: " + base64String);

            // Show picture
            ButtonState = false;
            IsImagePopOpen = true;

            // Do REST magic 
            await Task.Delay(1500);

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
            IsImagePopOpen = false;
            IsResultPopOpen = true;

            // Return
            await Task.Delay(2000);
            IsResultPopOpen = false;
            ButtonState = true;
        }

    }
}