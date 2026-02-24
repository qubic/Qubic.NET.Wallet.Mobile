using BarcodeScanning;

namespace Qubic.Net.Wallet.Mobile;

/// <summary>
/// Opens a native camera scanner page and returns the scanned QR code value.
/// </summary>
public sealed class QrScannerService
{
    private TaskCompletionSource<string?>? _tcs;

    public async Task<string?> ScanAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
                return null;
        }

        _tcs = new TaskCompletionSource<string?>();
        var scanPage = new ScanPage(this);

        if (Application.Current?.Windows.FirstOrDefault()?.Page is { } page)
        {
            await page.Navigation.PushModalAsync(scanPage);
        }
        else
        {
            _tcs.TrySetResult(null);
        }

        return await _tcs.Task;
    }

    internal void SetResult(string? value)
    {
        _tcs?.TrySetResult(value);
    }
}
