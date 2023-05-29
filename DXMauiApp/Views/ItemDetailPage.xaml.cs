using DXMauiApp.ViewModels;

namespace DXMauiApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new ItemDetailViewModel();
            ViewModel.OnAppearing();
        }

        ItemDetailViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}