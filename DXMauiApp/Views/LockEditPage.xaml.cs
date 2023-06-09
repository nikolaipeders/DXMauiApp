using DXMauiApp.ViewModels;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LockEditPage : ContentPage
    {
        public LockEditPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new LockEditViewModel();
            ViewModel.OnAppearing();
        }

        LockEditViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}