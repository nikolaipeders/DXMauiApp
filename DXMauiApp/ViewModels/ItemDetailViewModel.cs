using DXMauiApp.Models;
using DXMauiApp.Services;
using Plugin.Maui.Audio;
using System.Diagnostics;
using System.Web;

namespace DXMauiApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel, IQueryAttributable
    {
        public const string ViewName = "ItemDetailPage";

        public string Id { get; set; }

        string lockTitle;
        public string LockTitle
        {
            get => this.lockTitle;
            set => SetProperty(ref this.lockTitle, value);
        }

        string description;
        public string Description
        {
            get => this.description;
            set => SetProperty(ref this.description, value);
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

        public ItemDetailViewModel()
        {

            TakeSnapshotCmd = new Command(TakePhoto);

            OpenActionSheetCmd = new Command(() =>
            {
                IsActionSheetOpen = !IsActionSheetOpen;
            });
        }

        public async Task LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                LockTitle = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to Load Item");
            }
        }

        public override async Task InitializeAsync(object parameter)
        {
            await LoadItemId(parameter as string);
        }

        public void OnAppearing()
        {
            ResetState();

            GetDetails();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            string id = HttpUtility.UrlDecode(query["id"] as string);
            await LoadItemId(id);
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
                Email = Mail,
                Image = "data:image/jpeg;base64," + base64String
            };

            Debug.WriteLine("LOOK AT MAIL:" + user.Email);

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
            Title = LockTitle;
            SnapShot = "smartdoor.png";
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
            }
        }
    }
}