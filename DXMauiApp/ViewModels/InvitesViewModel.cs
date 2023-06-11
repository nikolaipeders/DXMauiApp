using CommunityToolkit.Mvvm.Messaging;
using DXMauiApp.Models;
using DXMauiApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace DXMauiApp.ViewModels
{
    public class InvitesViewModel : BaseViewModel
    {
        Invite selectedInvite;
        public Invite SelectedInvite
        {
            get => this.selectedInvite;
            set => SetProperty(ref this.selectedInvite, value);
        }

        TokenRequest token;
        public TokenRequest Token
        {
            get => this.token;
            set => SetProperty(ref this.token, value);
        }

        User exiUser;
        public User ExiUser
        {
            get => this.exiUser;
            set => SetProperty(ref this.exiUser, value);
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

        public ObservableCollection<Invite> Invites { get; set; }
        public Command AcceptInviteCommand { get; }
        public Command DeclineInviteCommand { get; }
        public Command<Invite> InviteTapped { get; }
        public Command CloseActionSheetCmd { get; set; }

        public InvitesViewModel()
        {
            Title = "Invites";
            Token = new TokenRequest();
            ExiUser = new User();
            SelectedInvite = new Invite();
            Invites = new ObservableCollection<Invite>();
            InviteTapped = new Command<Invite>(OnInviteSelected);

            CloseActionSheetCmd = new Command(() =>
            {
                IsActionSheetOpen = false;
                IsOwner = false;
            });

            AcceptInviteCommand = new Command(async () =>
            {
                await SendAnswer("accept");
            });

            DeclineInviteCommand = new Command(async () =>
            {
                await SendAnswer("deny");
            });
        }

        public async void OnAppearing()
        {
            RedirectToLogin();

            await GetDetails();

            await LoadInvites();

            SelectedInvite = null;
        }

        async Task LoadInvites()
        {
            try
            {
                var invites = await InviteService.GetAllInvitesAsync(Token, ExiUser._id);

                Invites.Clear();

                foreach (var item in invites)
                {
                    if (!item.accepted)
                    {
                        DateTime dateTime = DateTime.ParseExact(item.date, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
                        item.date = dateTime.ToString("dd-MM-yyyy 'at' HH:mm");

                        Invites.Add(item);
                    }
                }

                ObservableCollection<Invite> uniqueInvites = new ObservableCollection<Invite>(Invites.Distinct());

                Invites = uniqueInvites;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        async Task SendAnswer(string answer)
        {
            try
            {
                var result = await InviteService.RespondToInviteAsync(Token, SelectedInvite._id, answer);
                await LoadInvites();
                IsActionSheetOpen = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        void OnInviteSelected(Invite invite)
        {
            if (invite == null)
                return;

            if (!IsActionSheetOpen)
            {
                SelectedInvite = invite;
                IsActionSheetOpen = true;
            }
        }

        public async Task GetDetails()
        {
            var authToken = await SecureStorage.Default.GetAsync("auth_token");
            Token.Token = authToken.Replace("\"", "");

            Id = await SecureStorage.Default.GetAsync("user_id");

            ExiUser = await UserService.GetUserByIdAsync(Token, Id);

            WeakReferenceMessenger.Default.Send(new MessagePublisher(Id, Token.Token));
        }
    }
}