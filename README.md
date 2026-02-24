# Qubic Mobile Wallet

> [!IMPORTANT] 
> **Lab Project** — This is an experimental/research project and is **not intended for production use**. Use at your own risk. Do not use with significant funds.

A cross-platform mobile wallet for the Qubic network, built with .NET MAUI Blazor Hybrid.

## Features

- **Vault Management** — Encrypted vault with multiple seeds, contacts address book, import/export, biometric unlock (fingerprint/face)
- **Send QU** — Transfer QU tokens with transaction tracking, auto-resend, and tick offset configuration
- **Receive / QR Code** — Generate and share QR codes for your wallet address
- **Asset Management** — View owned assets with details and transfer capability
- **DeFi Hub** — Access QEarn staking, QSwap, MSVault multi-sig, and QX exchange
- **Transaction History** — Merged view of tracked and synced transactions with status badges, filters, and pagination
- **Log Events** — Browse synced log events (transfers, asset operations, contract messages) with type filters
- **Sync Manager** — Real-time sync status, stream diagnostics, database stats, and watermark management
- **QR Scanner** — Native camera-based QR scanning on all address input fields
- **Settings** — Backend configuration (RPC/Bob/Direct), transaction parameters, data import/export

## Architecture

The app uses a **Blazor Hybrid** approach: native MAUI shell with a `BlazorWebView` rendering the UI. All business logic lives in the shared `Qubic.Services` library (included as a git submodule), which is the same codebase used by the desktop wallet.

```
Qubic.Net.Wallet.Mobile/
├── Components/
│   ├── Layout/          # MobileLayout (nav bar, tab bar)
│   ├── Pages/           # Blazor pages (Home, Send, Assets, DeFi, Vault, ...)
│   └── Shared/          # Reusable components (AddressInput, ConfirmModal, ...)
├── Services/            # Mobile-specific services (QrScannerService)
├── Pages/               # Native MAUI pages (ScanPage for camera)
├── Platforms/android/   # Android manifest, MainActivity
├── Resources/           # App icon, splash screen, fonts
├── wwwroot/             # Static assets (CSS, JS, SVG logos)
└── deps/Qubic.Net/      # Git submodule → shared library
```

### Key Dependencies

| Package | Purpose |
|---------|---------|
| `Microsoft.Maui.Controls` | .NET MAUI framework |
| `Microsoft.AspNetCore.Components.WebView.Maui` | Blazor Hybrid WebView |
| `Oscore.Maui.Biometric` | Fingerprint/face authentication |
| `BarcodeScanning.Native.Maui` | Native QR code scanning (ML Kit) |
| `QRCoder` | QR code generation |
| `SQLitePCLRaw.bundle_e_sqlcipher` | Encrypted SQLite storage |

## Building

### Prerequisites

- .NET 9 SDK
- Android SDK (API 24+, build tools for API 35)
- Java 11 JDK

### Build APK

```bash
dotnet publish -f net9.0-android -c Release
```

The signed APK is output to:
```
bin/Release/net9.0-android/publish/li.qubic.wallet-Signed.apk
```

### Install on Device/Emulator

```bash
adb install -r bin/Release/net9.0-android/publish/li.qubic.wallet-Signed.apk
```

## Setup After Clone

The shared library is a git submodule. After cloning, initialize it:

```bash
git submodule update --init --recursive
```

