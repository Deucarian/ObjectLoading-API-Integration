using System.Threading;
using System.Threading.Tasks;
using JorisHoef.APIHelper.Core;
using JorisHoef.APIHelper.Models;

namespace JorisHoef.ObjectLoading.APIHelperBridge.Tests
{
    internal sealed class RecordingApiClient : IApiClient
    {
        public ApiRequest LastRequest { get; private set; }
        public ApiResult<byte[]> NextBytesResult { get; set; }

        public Task<ApiResult<TResponse>> SendAsync<TResponse>(ApiRequest request,
                                                               CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            if (typeof(TResponse) == typeof(byte[]))
            {
                return Task.FromResult((ApiResult<TResponse>)(object)NextBytesResult);
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
