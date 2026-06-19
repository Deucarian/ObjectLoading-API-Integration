# API AssetBundle Loader Sample

`ApiIntegrationDownloaderSample.cs` shows the recommended composition for API-backed AssetBundle loading:

- create or inject an `IApiClient`
- create the Object Loading pipeline through `ApiObjectLoadingPipelineFactory`
- pass bearer auth on the `ObjectLoadRequest`
- receive Object Loading progress and diagnostics
- unload the returned object handle during cleanup

The sample still expects a resolved AssetBundle URL. It does not resolve backend URLs or move backend route construction into the integration.

`ApiObjectDownloader` remains available only for explicit byte-array workflows.
