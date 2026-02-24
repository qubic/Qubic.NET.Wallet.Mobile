# Release Notes

## v0.2.0

### Core Features
- **Vault Management** — Create, lock, unlock, and manage an encrypted vault (PBKDF2 + AES-256-GCM) with multiple seeds and a contacts address book
- **Biometric Unlock** — Unlock vault with fingerprint or face authentication (Android BiometricPrompt)
- **Send QU** — Transfer QU tokens with transaction tracking, auto-resend on failure, and configurable tick offset
- **Receive / QR Code** — Generate and share QR codes for your wallet address
- **Asset Management** — View owned assets with details, issuer info, and asset transfer capability
- **QR Scanner** — Native camera-based QR code scanning on all address input fields

### DeFi Hub
- QEarn staking interface
- QSwap token swap interface
- MSVault multi-signature vault interface
- QX exchange interface

### History & Logs
- Merged transaction view combining tracked (pending) and synced transactions
- Transaction status badges (Pending, Confirmed, Failed, Executed)
- Direction and hash type filters with pagination
- Log events tab with type filters (Transfer, Asset, Contract, Burn), epoch and tx hash search
- Export/import tracked transactions as JSON

### Sync & Settings
- Sync Manager with real-time stream status, progress bars, and database stats
- Backend configuration (RPC, Bob, Direct Network)
- Transaction parameters (tick offset, auto-resend, max retries)
- Database export/import, clear transactions, clear logs, reset sync

### UI/UX
- Qubic logo app icon and splash screen (adaptive icon for Android)
- Seamless splash-to-app transition (no "Loading..." flash)
- Dark theme with Qubic cyan accents
- Card-based mobile layout with bottom tab navigation
- Vault status indicator in top bar
- "No wallet open" guard on all data pages when vault is locked
- Data section hidden in Settings when no wallet is open

### Technical
- .NET MAUI 9 Blazor Hybrid (net9.0-android, API 24+)
- Shared business logic via Qubic.Services git submodule
- Encrypted SQLite storage (SQLCipher)
- Native barcode scanning (ML Kit via BarcodeScanning.Native.Maui)
- Biometric auth (Oscore.Maui.Biometric)
- QR generation (QRCoder)
