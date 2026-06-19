using System;
using System.Threading;
using System.Threading.Tasks;
using Deucarian.API.Core;
using Deucarian.API.Models;
using UnityEngine;

namespace Deucarian.ObjectLoading.APIIntegration.Tests
{
    internal sealed class RecordingApiClient : IApiClient
    {
        public ApiRequest LastRequest { get; private set; }
        public Type LastResponseType { get; private set; }
        public int ByteArrayRequestCount { get; private set; }
        public int AssetBundleRequestCount { get; private set; }
        public ApiResult<byte[]> NextBytesResult { get; set; }
        public ApiResult<AssetBundle> NextAssetBundleResult { get; set; }
        public Action<ApiRequest> OnSend { get; set; }

        public Task<ApiResult<TResponse>> SendAsync<TResponse>(ApiRequest request,
                                                               CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            LastResponseType = typeof(TResponse);
            OnSend?.Invoke(request);

            if (typeof(TResponse) == typeof(byte[]))
            {
                ByteArrayRequestCount++;
                return Task.FromResult((ApiResult<TResponse>)(object)NextBytesResult);
            }

            if (typeof(TResponse) == typeof(AssetBundle))
            {
                AssetBundleRequestCount++;
                ApiResult<AssetBundle> result = NextAssetBundleResult
                                                ?? ApiResult<AssetBundle>.Failure(
                                                    new ApiError { Message = "No AssetBundle result configured." },
                                                    Deucarian.API.HttpMethod.GET);
                return Task.FromResult((ApiResult<TResponse>)(object)result);
            }

            return Task.FromResult(new ApiResult<TResponse>
            {
                IsSuccess = false,
                ErrorMessage = "Unexpected response type."
            });
        }

        public Task<ApiResult<TResponse>> SendAsync<TResponse>(ApiEndpoint endpoint,
                                                               CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }

        public Task<ApiResult<TResponse>> SendAsync<TResponse>(ApiEndpoint endpoint,
                                                               object body,
                                                               CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }

        public Task<ApiResult<TResponse>> GetAsync<TResponse>(string endpoint,
                                                              CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }

        public Task<ApiResult<TResponse>> PostAsync<TResponse>(string endpoint,
                                                               object body,
                                                               CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }

        public Task<ApiResult<TResponse>> PutAsync<TResponse>(string endpoint,
                                                              object body,
                                                              CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }

        public Task<ApiResult<TResponse>> PatchAsync<TResponse>(string endpoint,
                                                                object body,
                                                                CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }

        public Task<ApiResult<TResponse>> DeleteAsync<TResponse>(string endpoint,
                                                                 CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApiResult<TResponse> { IsSuccess = false });
        }
    }
}
