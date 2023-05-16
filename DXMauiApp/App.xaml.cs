using DXMauiApp.Services;
using DXMauiApp.ViewModels;
using DXMauiApp.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;

namespace DXMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<NavigationService>();

            Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
            Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
            Routing.RegisterRoute(typeof(LoginPage).FullName, typeof(LoginPage));
            Routing.RegisterRoute(typeof(RegisterPage).FullName, typeof(RegisterPage));
            MainPage = new MainPage();
            // Use the NavigateToAsync<ViewModelName> method
            // to display the corresponding view.
            // Code lines below show how to navigate to a specific page.
            // Comment out all but one of these lines
            // to open the corresponding page.
            //var navigationService = DependencyService.Get<INavigationService>();
            //navigationService.NavigateToAsync<LoginViewModel>(true);
        }
    }
}
