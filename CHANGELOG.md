# Changelog

## 0.1.2 - 2026-06-17

- Renamed the package identity from `com.deucarian.object-loading.api-bridge` to `com.deucarian.object-loading.api-integration`.
- Renamed APIBridge assemblies, namespaces, tests, and samples to APIIntegration.
- Migration: remove the old bridge package ID from Unity manifests and add `com.deucarian.object-loading.api-integration`.

## 0.1.1

- Updated the Object Loading dependency to `0.4.1`.
- Kept Newtonsoft Json aligned at `3.2.2`.
- Added package license metadata.
- Updated README structure with overview, installation, usage, samples, tests, and license sections.

## 0.1.0

- Added initial API Helper integration package metadata.
- Added `ApiHelperObjectDownloader`.
- Added request/result/error mapping helpers.
- Added EditMode tests for auth/header forwarding, byte result mapping, error mapping, and debug redaction.
- Added sample composition script.
