# Deucarian Object Loading API Bridge

## Overview

`com.deucarian.object-loading.api-bridge` is an optional UPM package that adapts API to Deucarian Object Loading.

The core `com.deucarian.object-loading` package has no API dependency. Use this bridge only when downloads should go through API request handling.

## What It Does

- Implements `IObjectDownloader` as `ApiObjectDownloader`.
- Converts `ObjectLoadRequest` URL, headers, bearer token, timeout, and cancellation into an API `ApiRequest`.
- Requests `ApiResponseFormat.Bytes`.
- Maps `ApiResult<byte[]>` back to `ObjectDownloadResult`.
- Maps API errors to `ObjectLoadError`.
- Keeps auth token passing explicit through `ObjectLoadRequest.BearerToken`, `Authorization: Bearer ...`, or an explicitly composed API client.

## What It Does Not Do

- No Session dependency.
- No backend URL resolving.
- No backend DTOs.
- No glTF loading.
- No Addressables integration.
- No caching policy.
- No material remapping.

Backend URL resolving remains outside this bridge. Resolve the final URL elsewhere, then pass it into `ObjectLoadRequest`.

## Installation

Install this bridge after installing its dependencies:

- `com.deucarian.object-loading`
- `com.deucarian.api`

The package depends on `com.deucarian.object-loading` `0.4.1`, `com.deucarian.api` `1.0.0`, and Unity's Newtonsoft Json package `3.2.2`.

## Usage

```csharp
using Deucarian.ObjectLoading;
using Deucarian.ObjectLoading.APIBridge;

ObjectLoadingPipeline pipeline = new ObjectLoadingPipeline(
    new DirectUrlSourceResolver(),
    new ApiObjectDownloader(),
    new AssetBundleContentLoader(),
    new AssetBundleObjectInstantiator(),
    new DefaultObjectDiagnostics());

ObjectLoadRequest request = ObjectLoadRequest.FromUrl(assetBundleUrl);
request.BearerToken = accessToken;
request.AddHeader("X-Custom-Header", "value");
```

For explicit composition and tests, pass your own API client:

```csharp
ApiObjectDownloader downloader = new ApiObjectDownloader(apiClient);
```

## Auth And Headers

`ObjectLoadRequest.BearerToken` becomes `ApiRequest.BearerTokenOverride`.

An explicit `Authorization: Bearer ...` header is also parsed into `BearerTokenOverride`. Other headers are forwarded unchanged.

`ApiObjectDownloadMapper.CreateDebugSnapshotJson(...)` redacts bearer tokens and sensitive headers.

## Samples

Import the **API Downloader Sample** from Unity's Package Manager to see `ApiObjectDownloader` composed into an `ObjectLoadingPipeline`.

## Tests

Run the package's EditMode tests in Unity. Tests cover auth/header forwarding, byte-result mapping, error mapping, and debug redaction.

## License

See [LICENSE.md](LICENSE.md).
