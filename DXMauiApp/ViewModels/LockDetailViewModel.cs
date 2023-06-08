using DXMauiApp.Models;
using DXMauiApp.Services;
using Plugin.Maui.Audio;
using System.Diagnostics;
using System.Globalization;
using System.Web;

namespace DXMauiApp.ViewModels
{
    public class LockDetailViewModel : BaseViewModel
    {
        public const string ViewName = "LockDetailPage";

        TokenRequest token;
        public TokenRequest Token
        {
            get => this.token;
            set => SetProperty(ref this.token, value);
        }

        string id;
        public string Id
        {
            get => this.id;
            set => SetProperty(ref this.id, value);
        }

        string lockId;
        public string LockId
        {
            get => this.lockId;
            set => SetProperty(ref this.lockId, value);
        }

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

        string mail;
        public string Mail
        {
            get => this.mail;
            set => SetProperty(ref this.mail, value);
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

        bool isActionSheetOpen = false;
        public bool IsActionSheetOpen
        {
            get => this.isActionSheetOpen;
            set
            {
                SetProperty(ref this.isActionSheetOpen, value);
            }
        }

        public Command TakeSnapshotCmd { get; set; }
        public Command OpenActionSheetCmd { get; set; }

        public LockDetailViewModel()
        {
            Token = new TokenRequest();

            TakeSnapshotCmd = new Command(TakePhoto);

            OpenActionSheetCmd = new Command(() =>
            {
                IsActionSheetOpen = !IsActionSheetOpen;
            });
        }

        public void OnAppearing()
        {
            ResetState();

            MessagingCenter.Subscribe<LocksViewModel, (string, string)>(this, "TransferTokenAndId", OnTransferRegistered);

            MessagingCenter.Subscribe<LocksViewModel, string>(this, "TransferLockId", (sender, lockId) =>
            {
                LockId = lockId;
            });

            Debug.WriteLine("LOCK ID IS " + LockId + lockId);

        }

        private void OnTransferRegistered(LocksViewModel sender, (string, string) transferInfo)
        {
            Token.Token = transferInfo.Item1;

            Debug.WriteLine("TRANSFER TOKEN IS " + Token.Token + transferInfo.Item1 + transferInfo.Item2);
        }

        public async Task LoadLockById(string itemId)
        {
            try
            {
                var item = await LockService.GetLockByIdAsync(Token, itemId);
                Id = item._id;
                Name = item.name;
                Location = item.location;

                var authToken = await SecureStorage.Default.GetAsync("auth_token");
                Token.Token = authToken.Replace("\"", "");

            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to Load Item");
            }
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

                    //SnapShot = ImageSource.FromFile(localFilePath);

                    // Convert photo file to byte array
                    byte[] photoBytes = File.ReadAllBytes(localFilePath);

                    // Encode the byte array as base64
                    string base64Photo = Convert.ToBase64String(photoBytes);

                    await Task.Delay(250);
                    UnlockDoor(base64Photo);
                }
            }
        }

        public async void UnlockDoor(string base64String)
        {
            // Prepare sound effects
            AudioManager am = new AudioManager();

            // State handling
            ButtonState = false;

            // Show loading screen
            IsLoading = true;
            ImageUrl = "";
            ImageDescription = "Verifying...";
            IsResultPopOpen = true;

            // Do REST magic
            User user = new User()
            {
                email = Mail,
                image = "data:image/jpeg;base64," + base64String
            };

            Debug.WriteLine("LOOK AT MAIL:" + user.email);

            var response = await UserService.VerifyUserByFaceAsync(user);

            IsLoading = false;

            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    ImageUrl = "door.png";
                    ImageDescription = "Door unlocked!";
                    var audioSuccess = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alert.mp3"));
                    audioSuccess.Play();
                }
                else
                {
                    ImageUrl = "error.png";
                    ImageDescription = "Access denied!";
                    var audioError = am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("wrong.mp3"));
                    audioError.Play();
                }
            }
            else
            {
                ImageUrl = "error.png";
                ImageDescription = "Error!";
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
            IsActionSheetOpen = false;
            IsLoading = false;
            ButtonState = true;
            Title = Name;
            SnapShot = "smartdoor.png";
        }
    }
}