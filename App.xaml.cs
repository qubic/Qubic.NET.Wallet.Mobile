using Qubic.Net.Wallet.Mobile.Services;

namespace Qubic.Net.Wallet.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage());

        var lifecycle = IPlatformApplication.Current?.Services.GetService<AppLifecycleService>();
        if (lifecycle != null)
        {
            window.Resumed += (_, _) => lifecycle.NotifyResumed();
            window.Stopped += (_, _) => lifecycle.NotifyStopped();
        }

        return window;
    }
}
