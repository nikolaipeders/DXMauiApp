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

            DependencyService.Register<NavigationService>();
            DependencyService.Register<UserRestService>();
            DependencyService.Register<LockRestService>();
            DependencyService.Register<InviteRestService>();
            DependencyService.Register<ServerRestService>();

            Routing.RegisterRoute(typeof(LockDetailPage).FullName, typeof(LockDetailPage));
            Routing.RegisterRoute(typeof(LockEditPage).FullName, typeof(LockEditPage));
            Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
            Routing.RegisterRoute(typeof(InvitesPage).FullName, typeof(InvitesPage));
            Routing.RegisterRoute(typeof(LoginPage).FullName, typeof(LoginPage));
            Routing.RegisterRoute(typeof(RegisterPage).FullName, typeof(RegisterPage));

            MainPage = new MainPage();
        }
    }
}
