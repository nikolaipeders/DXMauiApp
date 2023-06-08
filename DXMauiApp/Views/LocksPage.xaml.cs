using DXMauiApp.ViewModels;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocksPage : ContentPage
    {
        public LocksPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new LocksViewModel();
            ViewModel.OnAppearing();
        }

        LocksViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}