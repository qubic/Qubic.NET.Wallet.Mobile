using Android.App;
using Android.Content.PM;

namespace Qubic.Net.Wallet.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    WindowSoftInputMode = Android.Views.SoftInput.AdjustResize,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize
    | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
