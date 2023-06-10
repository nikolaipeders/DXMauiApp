using DXMauiApp.ViewModels;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InvitesPage : ContentPage
    {
        public InvitesPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new InvitesViewModel();
            ViewModel.OnAppearing();
        }

        InvitesViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}