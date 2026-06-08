using System;
using System.Collections;
using System.Threading.Tasks;
using JorisHoef.APIHelper.Core;
using JorisHoef.APIHelper.Models;
using JorisHoef.ObjectLoading;

namespace JorisHoef.ObjectLoading.APIHelperBridge
{
    public sealed class ApiHelperObjectDownloader : IObjectDownloader
    {
        private readonly IApiClient _apiClient;

        public ApiHelperObjectDownloader()
            : this(ApiClientFactory.CreateDefault())
        {
        }

        public ApiHelperObjectDownloader(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public IEnumerator DownloadAsync(ObjectSource source,
                                         ObjectLoadRequest request,
                                         Action<ObjectDownloadResult> onCompleted)
        {
            ObjectLoadError validationError = ApiHelperObjectDownloadMapper.Validate(source);
            if (validationError != null)
            {
                onCompleted?.Invoke(ObjectDownloadResult.Failure(validationError));
                yield break;
            }

            ApiRequest apiRequest = ApiHelperObjectDownloadMapper.CreateApiRequest(source, request);
            Task<ApiResult<byte[]>> task = _apiClient.SendAsync<byte[]>(
                apiRequest,
                request != null ? request.CancellationToken : default);

            while (!task.IsCompleted)
            {
                if (request != null && request.CancellationToken.IsCancellationRequested)
                {
                    onCompleted?.Invoke(ObjectDownloadResult.Failure(ObjectLoadError.Create(
                        ObjectLoadErrorCode.Canceled,
                        "API Helper AssetBundle download was canceled.",
                        source.Url)));
                    yield break;
                }

                yield return null;
            }

            if (task.IsCanceled)
            {
                onCompleted?.Invoke(ObjectDownloadResult.Failure(ObjectLoadError.Create(
                    ObjectLoadErrorCode.Canceled,
                    "API Helper AssetBundle download was canceled.",
                    source.Url)));
                yield break;
            }

            if (task.IsFaulted)
            {
                Exception exception = task.Exception != null ? task.Exception.GetBaseException() : null;
                onCompleted?.Invoke(ObjectDownloadResult.Failure(ObjectLoadError.Create(
                    ObjectLoadErrorCode.DownloadFailed,
                    "API Helper AssetBundle download failed: " + (exception != null ? exception.Message : "Unknown error."),
                    source.Url,
                    null,
                    exception != null ? exception.Message : null)));
                yield break;
            }

            onCompleted?.Invoke(ApiHelperObjectDownloadMapper.MapApiResult(task.Result, source.Url));
        }
    }
}
