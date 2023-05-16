using DXMauiApp.Views;
using System.Windows.Input;

namespace DXMauiApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public AccountViewModel()
        {
            Title = "Account";
            OpenLoginPage = new Command(async () => await Navigation.NavigateToAsync<LoginViewModel>(null));
        }

        public ICommand OpenLoginPage { get; }
    }
}