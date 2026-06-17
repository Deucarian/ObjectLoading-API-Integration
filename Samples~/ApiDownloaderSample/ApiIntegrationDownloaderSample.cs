using System.Collections;
using Deucarian.ObjectLoading;
using Deucarian.ObjectLoading.APIIntegration;
using UnityEngine;

public sealed class ApiIntegrationDownloaderSample : MonoBehaviour
{
    [SerializeField] private string assetBundleUrl;
    [SerializeField] private string bearerToken;
    [SerializeField] private Transform parent;

    private ObjectLoadingPipeline _pipeline;
    private IObjectLoadHandle _handle;

    private void Awake()
    {
        _pipeline = new ObjectLoadingPipeline(
            new DirectUrlSourceResolver(),
            new ApiObjectDownloader(),
            new AssetBundleContentLoader(),
            new AssetBundleObjectInstantiator(),
            new DefaultObjectDiagnostics());
    }

    public IEnumerator Load()
    {
        _handle?.Unload();
        ObjectLoadRequest request = ObjectLoadRequest.FromUrl(assetBundleUrl);
        request.BearerToken = bearerToken;
        request.Parent = parent;

        ObjectLoadResult result = null;
        yield return _pipeline.LoadAsync(request, value => result = value);

        if (result != null && result.Succeeded)
        {
            _handle = result.Handle;
            Debug.Log(result.Diagnostics.ToText());
        }
        else
        {
            Debug.LogError(result != null ? result.Message : "Object load finished without a result.");
        }
    }

    public void Unload()
    {
        _handle?.Unload();
        _handle = null;
        _pipeline?.UnloadLast();
    }

    private void OnDestroy()
    {
        Unload();
    }
}
