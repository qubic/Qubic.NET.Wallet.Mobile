# Release Notes

## v0.7.1

### Security
- **Seed generation now uses the operating system's secure random generator.** New seeds (create wallet, add account, add seed to vault) were built with .NET's general-purpose `System.Random`. That isn't an API meant for keys. On the .NET versions this app has always used (.NET 6 and later), each `new Random()` starts from 256 bits taken from the operating system's secure random source and was used only for that one seed, so we know of no practical way to predict seeds created by earlier versions. Seeds now come from `QubicSeed.Generate()` in `Qubic.Core`, the same generator the desktop wallet and the Toolkit use: secure randomness, every letter equally likely.

### Features
- **Networks.** Switch between Mainnet, a local AIO Qubic Dev Kit and your own networks under *More → Networks*. Each network uses Bob, RPC and/or a direct node connection, chosen per operation, and never falls back to servers outside the network. History, assets and settings are kept per network; any vault works on any network.
- **Network check.** The wallet identifies the chain it is connected to (Mainnet, or a core-lite devnet) and warns when it doesn't match the network's settings. Chain resets on test networks are detected and stale history is cleared.
- **Signing review.** Every signature shows the network, the decoded contract call, the target tick and warnings before you sign, including replay warnings when a seed signs on both Mainnet and a test network, and red warnings for public dev keys on Mainnet.
- **Target tick by time.** Besides the fixed tick offset, transactions can target "in ~N seconds".
- **Payment requests.** *Receive* creates `qubic:` links and QR codes with an amount and a message; *Send* fills its form from a scanned or pasted link.
- **Optional features, off by default** (*More → Features*): Developer tools (dev accounts, faucet, Contract Console with C++ header import), Sign-In with Qubic, and WalletConnect with QR-code pairing.
- Local test networks: the emulator reaches the host as `10.0.2.2`; on a USB phone use `adb reverse tcp:40420 tcp:40420 && adb reverse tcp:41841 tcp:41841`.

### Notes
- App id is `org.qubic.lab.wallet`.
- Requires `Qubic.Core 1.301.9`, `Qubic.Services 1.6.0` and `Qubic.WalletConnect 1.0.0`.

### Downloads

| Platform | File |
|----------|------|
| Android | `org.qubic.lab.wallet-0.7.1.apk` |

---

## v0.6.3

### Security — please update immediately
- **FourQ low-order public-key forgery (CVE-class, exploited in the wild).** The verify path admitted public keys lying in FourQ's 392-element cofactor subgroup — most importantly the identity point `{1,0,0,0}`, which is also the QX contract address. For these keys an attacker can produce a deterministic signature that verifies for any message without knowing a private key. This was used on the live network to move QU from the identity point to attacker-controlled wallets. Patched in `Qubic.Crypto 1.5.2` (mirrors qubic/core PR #921) and pulled in transitively via `Qubic.Services 1.5.2`.
- No vault, on-disk, or wire-format changes. Pure verify-side hardening.

### Downloads

| Platform | File |
|----------|------|
| Android | `org.qubic.lab.wallet-0.6.3.apk` |

---

## v0.6.2

### Security
- **FourQ signature canonicalisation fix.** The verify path used to admit any scalar `S < 2^246`, but the curve order `N` is also less than `2^246`. Attackers could replay a valid signature `S` as its twin `S' = S + N` — same `(R, pubkey, message)`, so verify still succeeded, but the on-chain tx hash differed, bypassing dedup and enabling double-execution of the same intent. Patched in `Qubic.Crypto 1.5.1` (mirrors qubic/core commit `05f7348`) and pulled in transitively via `Qubic.Services 1.5.1`. No vault or on-disk format changes.

### Downloads

| Platform | File |
|----------|------|
| Android | `org.qubic.lab.wallet-0.6.2.apk` |

---

## v0.6.1

### Security
- **Vault key derivation upgraded to Argon2id** (`m=64 MiB, t=3, p=1`). Existing vaults written with PBKDF2-HMAC-SHA256 (600 000 iterations) continue to open and silently re-encrypt to Argon2id on the next save.
- **New vault file format (v2)** carries an explicit `Version`, `Kdf`, and `KdfParams` so future KDF changes can ship without breaking existing files. Legacy v1 envelopes remain readable.
- **Password validator now uses [zxcvbn](https://github.com/dropbox/zxcvbn) scoring** instead of the character-class checklist. Requires length ≥ 12 and zxcvbn score ≥ 3.

### UI
- **Live password-strength meter** on the Vault create and change-password screens, plus on the initial Create Vault flow from Home. Color-coded bar, strength label, zxcvbn warning, and improvement suggestions update as you type.

> ⚠️ Vaults saved by v0.6.1 cannot be opened by v0.5.x or earlier. Export/back up your vault before upgrading.

### Downloads

| Platform | File |
|----------|------|
| Android | `org.qubic.lab.wallet-0.6.1.apk` |

---

## v0.5.0

### Upgrade
- Upgraded to **.NET 10** and **MAUI 10** (net10.0-android)
- Synced Qubic.Net library to latest upstream
- Updated BarcodeScanning.Native.Maui to 3.0.3

### Fixes
- Fixed viewport overlapping Android status bar and navigation bar (safe area insets)
- Fixed SC Auctions page not found (route mismatch)
- Fixed deprecated `Rfc2898DeriveBytes` constructor (SYSLIB0060)

### Improvements
- **Send & Send to Many merged** into a single page with tabs
- **Top bar reworked** — balance displayed inline next to wallet name with compact format (mQU/bQU), epoch/tick replaced with tap-to-reveal connection indicator, removed non-functional vault icon
- **Central BalanceService** — balance stays in sync across all pages, auto-refreshes every 30 ticks and on identity switch

### Downloads

| Platform | File |
|----------|------|
| Android | `org.qubic.lab.wallet-0.5.0.apk` |

Verify downloads with the `.sha256` file included alongside the APK.

---

## v0.3.0

### New Features

#### MSVault
- Full multi-signature vault interface for creating, managing, and signing multi-sig transactions

#### Watchlist
- Dedicated watchlist page for monitoring addresses you don't own
- Add any 60-character identity with a custom label
- View QU balance and owned assets for each watched address
- Accessible from Vault > Manage menu

#### Light Theme
- Full light theme support for navigation, top bar, tab bar, cards, and all UI elements
- Theme selection is now persisted and applied on app startup (no longer resets to dark)

### Improvements

- **Vault UI restructured** — Consolidated from 5 separate cards to 2: Seeds card (with inline add form) and a single Manage menu card with expandable sections for contacts, change password, biometric toggle, and export
- **Asset page** — Enhanced asset display with improved caching (AssetCacheService) and better UX
- **Home page** — UI improvements
- **App icon** — Updated to new Qubic logo

### Downloads

| Platform | File |
|----------|------|
| Android | `org.qubic.lab.wallet-0.3.0.apk` |

Verify downloads with the `.sha256` file included alongside the APK.

---

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
