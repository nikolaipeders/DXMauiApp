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
            DependencyService.Register<UserRestService>();
            DependencyService.Register<LockRestService>();

            Routing.RegisterRoute(typeof(LockDetailPage).FullName, typeof(LockDetailPage));
            Routing.RegisterRoute(typeof(LockEditPage).FullName, typeof(LockEditPage));
            Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
            Routing.RegisterRoute(typeof(LoginPage).FullName, typeof(LoginPage));
            Routing.RegisterRoute(typeof(RegisterPage).FullName, typeof(RegisterPage));

            MainPage = new MainPage();
        }
    }
}
