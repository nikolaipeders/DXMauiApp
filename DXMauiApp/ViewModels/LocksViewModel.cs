using CommunityToolkit.Mvvm.Messaging;
using DXMauiApp.Models;
using DXMauiApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DXMauiApp.ViewModels
{
    public class LocksViewModel : BaseViewModel
    {
        Lock _selectedLock;
        public Lock SelectedLock
        {
            get => this._selectedLock;
            set
            {
                SetProperty(ref this._selectedLock, value);
                OnLockSelected(value);
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
        string lockId = string.Empty;
        public string LockId
        {
            get => this.lockId;
            set => SetProperty(ref this.lockId, value);
        }

        bool isOwner = false;
        public bool IsOwner
        {
            get => this.isOwner;
            set
            {
                SetProperty(ref this.isOwner, value);
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

        public ObservableCollection<Lock> Locks { get; set; }
        public Command LoadLocksCommand { get; }
        public Command NavigateToLockCommand { get; }
        public Command NavigateToEditorCommand { get; }
        public Command AddLockCommand { get; }
        public Command<Lock> LockTapped { get; }
        public Command CloseActionSheetCmd { get; set; }

        public LocksViewModel()
        {

            Title = "Locks";
            Token = new TokenRequest();
            Locks = new ObservableCollection<Lock>();
            LockTapped = new Command<Lock>(OnLockSelected);
            AddLockCommand = new Command(OnAddLock);

            CloseActionSheetCmd = new Command(() =>
            {
                IsActionSheetOpen = false;
                IsOwner = false;
            });

            NavigateToLockCommand = new Command(() => 
            {
                IsActionSheetOpen = false;
                SelectedLock = null;
                Navigation.NavigateToAsync<LockDetailViewModel>();
            });

            NavigateToEditorCommand = new Command(() =>
            {
                IsActionSheetOpen = false;
                SelectedLock = null;
                Navigation.NavigateToAsync<LockEditViewModel>();
            });
        }

        public async void OnAppearing()
        {
            RedirectToLogin();

            await GetDetails();

            await LoadLocks();

            SelectedLock = null;
        }

        async Task LoadLocks()
        {
            try
            {
                var locks = await LockService.GetAllLocksAsync(Token);

                // Update the existing Locks collection
                Locks.Clear();
                foreach (var item in locks)
                {
                    Locks.Add(item);
                }

                ObservableCollection<Lock> uniqueLocks = new ObservableCollection<Lock>(Locks.Distinct());
                Locks.Clear();
                foreach (var item in uniqueLocks)
                {
                    Locks.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //await CheckStatus();
            }
        }

        async void OnAddLock(object obj)
        {
            await Navigation.NavigateToAsync<NewItemViewModel>(null);
        }

        async void OnLockSelected(Lock exiLock)
        {
            if (exiLock == null)
                return;

            // Check if the action sheet is already open
            if (!IsActionSheetOpen)
            {
                await SecureStorage.Default.SetAsync("lock_id", exiLock._id);
                SelectedLock = exiLock;

                if (SelectedLock != null && SelectedLock.owner == Id || SelectedLock.owner == Id)
                {
                    IsOwner = true;
                }
                IsActionSheetOpen = true;
            }
        }

        public async Task GetDetails()
        {
            var authToken = await SecureStorage.Default.GetAsync("auth_token");
            var user_id = await SecureStorage.Default.GetAsync("user_id");

            if (authToken != null && user_id != null)
            {
                Token.Token = authToken.Replace("\"", "");
                Id = await SecureStorage.Default.GetAsync("user_id");

            }
            WeakReferenceMessenger.Default.Send(new MessagePublisher(Id, Token.Token));
        }

        public async Task CheckStatus()
        {
            while (true)
            {
                try
                {
                    var result = await ServerService.GetStatusAsync();

                    if (result != null && result.IsSuccessStatusCode)
                    {
                        IsResultPopOpen = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ImageUrl = "error.png";
                    ImageDescription = "Server is down";

                    IsResultPopOpen = true;
                }
                // Wait for 15 seconds before checking again
                await Task.Delay(15000);
            }
        }
    }
}