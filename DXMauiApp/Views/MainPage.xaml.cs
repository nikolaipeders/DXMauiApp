using DXMauiApp.ViewModels;
using System.Runtime.CompilerServices;

namespace DXMauiApp.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}