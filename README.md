# JorisHoef Object Loading API Helper Bridge

`com.jorishoef.object-loading.api-helper-bridge` is an optional UPM package that adapts API Helper to JorisHoef Object Loading.

The core `com.jorishoef.object-loading` package has no API Helper dependency. Use this bridge only when downloads should go through API Helper request handling.

## What It Does

- Implements `IObjectDownloader` as `ApiHelperObjectDownloader`.
- Converts `ObjectLoadRequest` URL, headers, bearer token, timeout, and cancellation into an API Helper `ApiRequest`.
- Requests `ApiResponseFormat.Bytes`.
- Maps `ApiResult<byte[]>` back to `ObjectDownloadResult`.
- Maps API Helper errors to `ObjectLoadError`.
- Keeps auth token passing explicit through `ObjectLoadRequest.BearerToken`, `Authorization: Bearer ...`, or an explicitly composed API Helper client.

## What It Does Not Do

- No Session Helper dependency.
- No backend URL resolving.
- No backend DTOs.
- No glTF loading.
- No Addressables integration.
- No caching policy.
- No material remapping.

Backend URL resolving remains outside this bridge. Resolve the final URL elsewhere, then pass it into `ObjectLoadRequest`.

## Usage

```csharp
using JorisHoef.ObjectLoading;
using JorisHoef.ObjectLoading.APIHelperBridge;

ObjectLoadingPipeline pipeline = new ObjectLoadingPipeline(
    new DirectUrlSourceResolver(),
    new ApiHelperObjectDownloader(),
    new AssetBundleContentLoader(),
    new AssetBundleObjectInstantiator(),
    new DefaultObjectDiagnostics());

ObjectLoadRequest request = ObjectLoadRequest.FromUrl(assetBundleUrl);
request.BearerToken = accessToken;
request.AddHeader("X-Custom-Header", "value");
```

For explicit composition and tests, pass your own API Helper client:

```csharp
ApiHelperObjectDownloader downloader = new ApiHelperObjectDownloader(apiClient);
```

## Auth And Headers

`ObjectLoadRequest.BearerToken` becomes `ApiRequest.BearerTokenOverride`.

An explicit `Authorization: Bearer ...` header is also parsed into `BearerTokenOverride`. Other headers are forwarded unchanged.

`ApiHelperObjectDownloadMapper.CreateDebugSnapshotJson(...)` redacts bearer tokens and sensitive headers.
