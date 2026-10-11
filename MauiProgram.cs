using Microsoft.Extensions.Logging;
using Maui.Biometric;
using BarcodeScanning;
using Qubic.Services;
using Qubic.Services.Contracts;
using Qubic.Services.DevTools;
using Qubic.Services.Features;
using Qubic.Services.Networks;
using Qubic.Services.Signing;
using Qubic.Services.Storage;
using Qubic.Net.Wallet.Mobile.Helpers;

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

        // Networks: profiles, egress guard (every connection must belong to the active profile), routing
        services.AddSingleton<NetworkProfileService>();
        services.AddSingleton<EgressGuard>();
        services.AddSingleton<NetworkChainState>();
        services.AddSingleton<QubicBackendService>();
        services.AddSingleton<NetworkFingerprintService>();
        services.AddSingleton<ChainResetService>();
        services.AddSingleton<NetworkSwitchService>();
        services.AddSingleton<FeatureService>();
        services.AddSingleton<ContractCatalogService>();
        services.AddSingleton<SigningReviewService>();
        services.AddSingleton<SeedSessionService>();
        services.AddSingleton<DevToolsService>();
        // dApp connectivity (opt-in; nothing runs until enabled)
        services.AddSingleton<ExternalRequestService>();
        services.AddSingleton(new Qubic.WalletConnect.WalletConnectOptions
        {
            Name = "Qubic.Net Wallet",
            Description = "Qubic.Net mobile wallet",
            // WalletConnect Cloud (Reown) project id of this app — public by design, not user-configurable.
            ProjectId = "bdd67eae89bb533af79f1210c44aaa5b",
        });
        services.AddSingleton<Qubic.WalletConnect.QubicWalletConnectService>();
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
        services.AddSingleton<AssetCacheService>();
        services.AddSingleton<BalanceService>();
        services.AddSingleton<Services.AppLifecycleService>();
    }
}
