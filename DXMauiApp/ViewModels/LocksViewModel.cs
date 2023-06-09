using CommunityToolkit.Mvvm.Messaging;
using DXMauiApp.Models;
using DXMauiApp.Services;
using Java.Util.Concurrent.Locks;
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
                Locks.Clear();

                var locks = await LockService.GetAllLocksAsync(Token);
                foreach (var item in locks)
                {
                    Locks.Add(item);
                }

                ObservableCollection<Lock> uniqueLocks = new ObservableCollection<Lock>(Locks.Distinct());
                Locks = uniqueLocks;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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

                if (SelectedLock != null && SelectedLock.owner == Id)
                {
                    IsOwner = true;
                }
                IsActionSheetOpen = true;
            }
        }

        public async Task GetDetails()
        {
            var authToken = await SecureStorage.Default.GetAsync("auth_token");
            Token.Token = authToken.Replace("\"", "");

            Id = await SecureStorage.Default.GetAsync("user_id");

            WeakReferenceMessenger.Default.Send(new MessagePublisher(Id, Token.Token));

            // Publish message with email and password
            Debug.WriteLine("LOCKSVIEWMODEL TOKEN.TOKEN IS " + Token.Token);
            Debug.WriteLine("LOCKSVIEWMODEL ID IS " + Id);

        }
    }
}