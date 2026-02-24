using BarcodeScanning;

namespace Qubic.Net.Wallet.Mobile;

public partial class ScanPage : ContentPage
{
    private readonly QrScannerService _scanner;
    private bool _handled;

    public ScanPage(QrScannerService scanner)
    {
        _scanner = scanner;
        InitializeComponent();
    }

    private async void OnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
    {
        if (_handled) return;
        var result = e.BarcodeResults?.FirstOrDefault();
        if (result == null) return;

        _handled = true;
        CameraView.CameraEnabled = false;
        _scanner.SetResult(result.DisplayValue);
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Navigation.PopModalAsync();
        });
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        if (_handled) return;
        _handled = true;
        CameraView.CameraEnabled = false;
        _scanner.SetResult(null);
        await Navigation.PopModalAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        if (!_handled)
        {
            _handled = true;
            CameraView.CameraEnabled = false;
            _scanner.SetResult(null);
            MainThread.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync());
        }
        return true;
    }
}
