#!/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$SCRIPT_DIR"

# Check required environment variables
if [ -z "$ANDROID_HOME" ]; then
    echo "ERROR: ANDROID_HOME is not set. Set it to your Android SDK path."
    echo "  e.g. export ANDROID_HOME=\$HOME/android-sdk"
    exit 1
fi

if [ -z "$JAVA_HOME" ]; then
    echo "ERROR: JAVA_HOME is not set. Set it to your Java 11+ JDK path."
    echo "  e.g. export JAVA_HOME=/usr/lib/jvm/java-11-openjdk-amd64"
    exit 1
fi

# Read version from csproj
VERSION=$(grep -oP '(?<=<ApplicationDisplayVersion>)[^<]+' Qubic.Net.Wallet.Mobile.csproj)
APP_ID="li.qubic.wallet"
APK_NAME="${APP_ID}-${VERSION}.apk"

echo "Building Qubic Mobile Wallet v${VERSION}..."

# Clean previous build artifacts
rm -rf obj/Release bin/Release deps/Qubic.Net/src/*/obj

# Build
dotnet publish -f net9.0-android -c Release \
    -p:RunAOTCompilation=false \
    -p:PublishTrimmed=false

# Prepare publish folder
mkdir -p publish
cp "bin/Release/net9.0-android/publish/${APP_ID}-Signed.apk" "publish/${APK_NAME}"

# Generate SHA-256 hash
cd publish
sha256sum "$APK_NAME" > "${APK_NAME}.sha256"

echo ""
echo "Done!"
echo "  APK:    publish/${APK_NAME}"
echo "  SHA256: publish/${APK_NAME}.sha256"
echo "  Hash:   $(cat "${APK_NAME}.sha256" | cut -d' ' -f1)"
