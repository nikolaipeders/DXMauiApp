using DXMauiApp.Models;
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
        public Command AddLockCommand { get; }
        public Command<Lock> LockTapped { get; }
        public Command OpenActionSheetCmd { get; set; }

        public LocksViewModel()
        {
            Title = "Locks";
            Token = new TokenRequest();
            Locks = new ObservableCollection<Lock>();
            LockTapped = new Command<Lock>(OnLockSelected);
            AddLockCommand = new Command(OnAddLock);

            OpenActionSheetCmd = new Command(() =>
            {
                IsActionSheetOpen = !IsActionSheetOpen;
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

            MessagingCenter.Send(this, "TransferLockId", exiLock._id);
            await Navigation.NavigateToAsync<LockDetailViewModel>(exiLock._id);
        }

        public async Task GetDetails()
        {
            var authToken = await SecureStorage.Default.GetAsync("auth_token");
            Token.Token = authToken.Replace("\"", "");

            Id = await SecureStorage.Default.GetAsync("user_id");

            // Publish message with email and password
            Debug.WriteLine("LOCKSVIEWMODEL TOKEN.TOKEN IS " + Token.Token);
            MessagingCenter.Send(this, "TransferTokenAndId", (Token.Token, Id));
        }
    }
}