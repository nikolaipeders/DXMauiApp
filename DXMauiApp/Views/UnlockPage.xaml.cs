using DXMauiApp.ViewModels;
using Plugin.Maui.Audio;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnlockPage : ContentPage
    {

        public UnlockPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new UnlockViewModel();
            ViewModel.OnAppearing();
        }

        UnlockViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}