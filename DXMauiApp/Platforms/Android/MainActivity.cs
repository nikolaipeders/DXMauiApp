using Android.App;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Content;
using Android.Util;
using Android.Nfc.Tech;

namespace DXMauiApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

    }
}