using DXMauiApp.ViewModels;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LockDetailPage : ContentPage
    {
        public LockDetailPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new LockDetailViewModel();
            ViewModel.OnAppearing();
        }

        LockDetailViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}