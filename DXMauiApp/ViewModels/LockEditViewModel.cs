using CommunityToolkit.Mvvm.Messaging;
using DXMauiApp.Models;
using DXMauiApp.Services;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Web;

namespace DXMauiApp.ViewModels
{
    public class LockEditViewModel : BaseViewModel
    {
        public const string ViewName = "LockDetailPage";

        User _selectedUser;
        public User SelectedUser
        {
            get => this._selectedUser;
            set => SetProperty(ref this._selectedUser, value);
        }


        Lock _selectedLock;
        public Lock SelectedLock
        {
            get => this._selectedLock;
            set => SetProperty(ref this._selectedLock, value);
        }

        TokenRequest token;
        public TokenRequest Token
        {
            get => this.token;
            set => SetProperty(ref this.token, value);
        }

        string userId;
        public string UserId
        {
            get => this.userId;
            set => SetProperty(ref this.userId, value);
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

        string serial;
        public string Serial
        {
            get => this.serial;
            set => SetProperty(ref this.serial, value);
        }

        bool isActive = false;
        public bool IsActive
        {
            get => this.isActive;
            set
            {
                SetProperty(ref this.isActive, value);
            }
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

        bool isUserActionSheetOpen = false;
        public bool IsUserActionSheetOpen
        {
            get => this.isUserActionSheetOpen;
            set
            {
                SetProperty(ref this.isUserActionSheetOpen, value);
            }
        }

        bool isDeleteActionSheetOpen = false;
        public bool IsDeleteActionSheetOpen
        {
            get => this.isDeleteActionSheetOpen;
            set
            {
                SetProperty(ref this.isDeleteActionSheetOpen, value);
            }
        }
        public ObservableCollection<User> Accessors { get; set; }
        public Command CloseUserActionSheetCmd { get; set; }
        public Command OpenDeleteActionSheetCmd { get; set; }
        public Command UserTapped { get; }

        public LockEditViewModel()
        {
            Token = new TokenRequest();
            SelectedLock = new Lock();
            SelectedUser = new User();

            Accessors = new ObservableCollection<User>();

            UserTapped = new Command<User>(OnUserSelected);

            CloseUserActionSheetCmd = new Command(() =>
            {
                IsUserActionSheetOpen = false;
            });

            OpenDeleteActionSheetCmd = new Command(() =>
            {
                IsDeleteActionSheetOpen = !IsDeleteActionSheetOpen;
            });

        }

        public async void OnAppearing()
        {
            ResetState();

            var authToken = await SecureStorage.Default.GetAsync("auth_token");
            Token.Token = authToken.Replace("\"", "");
            LockId = await SecureStorage.Default.GetAsync("lock_id");
            UserId = await SecureStorage.Default.GetAsync("user_id");

            SelectedUser = null;

            await LoadLockById();
        }

        public async Task LoadLockById()
        {
            try
            {
                SelectedLock = await LockService.GetLockByIdAsync(Token, LockId);
                Name = SelectedLock.name;
                Location = SelectedLock.location;
                IsActive = SelectedLock.active;

                Accessors.Clear();

                foreach (var user in SelectedLock.lock_access)
                {
                    User newUser = await UserService.GetUserByIdAsync(Token, user);
                    Accessors.Add(newUser);
                }
                ObservableCollection<User> uniqueAccessors = new ObservableCollection<User>(Accessors.Distinct());
                Accessors = uniqueAccessors;

            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to Load Item");
            }
        }

        async void OnInviteUser(object obj)
        {
            await Navigation.NavigateToAsync<NewItemViewModel>(null);
        }

        void OnUserSelected(User user)
        {
            if (user == null)
                return;

            // Check if the action sheet is already open
            if (!IsUserActionSheetOpen)
            {
                SelectedUser = user;
                IsUserActionSheetOpen = true;
            }
        }

        public void ResetState()
        {
            IsResultPopOpen = false;
            IsUserActionSheetOpen = false;
            IsDeleteActionSheetOpen = false;
            IsLoading = false;
            ButtonState = true;
            SnapShot = "smartdoor.png";
        }
    }
}