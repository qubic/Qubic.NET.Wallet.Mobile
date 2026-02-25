using Qubic.Core;
using Qubic.Services;

namespace Qubic.Net.Wallet.Mobile.Helpers;

public class AssetCacheService
{
    private readonly QubicBackendService _backend;
    private readonly SeedSessionService _seed;

    public AssetCacheService(QubicBackendService backend, SeedSessionService seed)
    {
        _backend = backend;
        _seed = seed;
        seed.OnSeedChanged += () => { Assets = null; LastIdentity = null; };
    }

    public List<CachedAsset>? Assets { get; private set; }
    public string? LastIdentity { get; private set; }
    public event Action? OnAssetsChanged;

    public async Task RefreshAsync()
    {
        if (_seed.Identity == null) return;
        var id = _seed.Identity.Value;
        var idStr = id.ToString();
        try
        {
            var owned = await _backend.GetOwnedAssetsAsync(id);
            Assets = owned?
                .Where(a => long.TryParse(a.Data.NumberOfUnits, out var u) && u > 0)
                .Select(a => new CachedAsset
                {
                    Name = a.Data.IssuedAsset?.Name ?? "Unknown",
                    Issuer = a.Data.IssuedAsset?.IssuerIdentity ?? a.Data.OwnerIdentity,
                    Amount = long.TryParse(a.Data.NumberOfUnits, out var n) ? n : 0,
                    ManagingContractIndex = (int)a.Data.ManagingContractIndex
                })
                .ToList() ?? [];
            LastIdentity = idStr;
            OnAssetsChanged?.Invoke();
        }
        catch { }
    }
}

public class CachedAsset
{
    public string Key => $"{Issuer}:{Name}";
    public string Name { get; set; } = "";
    public string Issuer { get; set; } = "";
    public long Amount { get; set; }
    public int ManagingContractIndex { get; set; }
}
