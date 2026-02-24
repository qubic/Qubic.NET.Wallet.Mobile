using Microsoft.Extensions.Logging;
using Maui.Biometric;
using BarcodeScanning;
using Qubic.Services;
using Qubic.Services.Storage;

namespace Qubic.Net.Wallet.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBiometricAuthentication()
            .UseBarcodeScanning()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

        RegisterServices(builder.Services);

        return builder.Build();
    }

    static void RegisterServices(IServiceCollection services)
    {
        // Defer QubicSettingsService so Android filesystem is ready
        services.AddSingleton(sp =>
        {
            SQLitePCL.Batteries_V2.Init();
            return new QubicSettingsService("QubicWallet");
        });

        services.AddSingleton<QubicBackendService>();
        services.AddSingleton<SeedSessionService>();
        services.AddSingleton<VaultService>();
        services.AddSingleton<TickMonitorService>();
        services.AddSingleton<TickDriftService>();
        services.AddSingleton<WalletDatabase>();
        services.AddSingleton<WalletSyncService>();
        services.AddSingleton<WalletStorageService>();
        services.AddSingleton<TransactionTrackerService>();
        services.AddSingleton<AssetRegistryService>();
        services.AddSingleton<PeerAutoDiscoverService>();
        services.AddSingleton<QubicStaticService>();
        services.AddSingleton<LabelService>();
        services.AddSingleton<QrScannerService>();
    }
}
