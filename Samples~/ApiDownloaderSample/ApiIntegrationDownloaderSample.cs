using System.Collections;
using Deucarian.API.Configuration;
using Deucarian.API.Core;
using Deucarian.ObjectLoading;
using Deucarian.ObjectLoading.APIIntegration;
using UnityEngine;

public sealed class ApiIntegrationDownloaderSample : MonoBehaviour
{
    [SerializeField] private string assetBundleUrl;
    [SerializeField] private string bearerToken;
    [SerializeField] private ApiClientConfig apiClientConfig;
    [SerializeField] private Transform parent;

    private ObjectLoadingPipeline _pipeline;
    private IObjectLoadHandle _handle;
    private IApiClient _apiClient;

    public string LastStatus { get; private set; }

    private void Awake()
    {
        if (_pipeline == null)
        {
            Initialize(ApiClientFactory.Create(apiClientConfig));
        }
    }

    public void Initialize(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _pipeline = ApiObjectLoadingPipelineFactory.Create(_apiClient);
    }

    public IEnumerator Load()
    {
        _handle?.Unload();
        ObjectLoadRequest request = ObjectLoadRequest.FromUrl(assetBundleUrl);
        request.BearerToken = bearerToken;
        request.Parent = parent;
        request.Progress = progress =>
        {
            if (progress.Phase == ObjectLoadPhase.Downloading)
            {
                LastStatus = "API AssetBundle progress: " + Mathf.RoundToInt(progress.Normalized * 100f) + "%";
            }
        };

        ObjectLoadResult result = null;
        yield return _pipeline.LoadAsync(request, value => result = value);

        if (result != null && result.Succeeded)
        {
            _handle = result.Handle;
            LastStatus = result.Diagnostics.ToText();
        }
        else
        {
            LastStatus = result != null ? result.Message : "Object load finished without a result.";
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
