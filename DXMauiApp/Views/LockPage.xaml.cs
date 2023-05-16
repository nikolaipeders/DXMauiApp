using DXMauiApp.ViewModels;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LockPage : ContentPage
    {
        public LockPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new LockViewModel();
            ViewModel.OnAppearing();
        }

        LockViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}