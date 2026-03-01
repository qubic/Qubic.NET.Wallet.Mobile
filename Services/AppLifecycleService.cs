namespace Qubic.Net.Wallet.Mobile.Services;

public sealed class AppLifecycleService
{
    public bool IsForegrounded { get; private set; } = true;
    public event Action? OnResumed;
    public event Action? OnStopped;

    public void NotifyResumed()
    {
        IsForegrounded = true;
        OnResumed?.Invoke();
    }

    public void NotifyStopped()
    {
        IsForegrounded = false;
        OnStopped?.Invoke();
    }
}
