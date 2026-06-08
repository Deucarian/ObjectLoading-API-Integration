using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using JorisHoef.APIHelper.Models;
using JorisHoef.ObjectLoading;

namespace JorisHoef.ObjectLoading.APIHelperBridge.Tests
{
    public sealed class ApiHelperObjectDownloaderTests
    {
        [Test]
        public void DownloadAsync_UsesApiClientAndMapsResult()
        {
            RecordingApiClient client = new RecordingApiClient
            {
                NextBytesResult = ApiResult<byte[]>.Success(
                    new byte[] { 9, 8, 7 },
                    JorisHoef.APIHelper.HttpMethod.GET,
                    200,
                    "https://example.com/object.bundle",
                    null)
            };
            ApiHelperObjectDownloader downloader = new ApiHelperObjectDownloader(client);
            ObjectLoadRequest request = ObjectLoadRequest.FromUrl("https://example.com/object.bundle");
            request.BearerToken = "token";
            ObjectDownloadResult result = null;

            Run(downloader.DownloadAsync(
                ObjectSource.DirectUrl(request.Url),
                request,
                value => result = value));

            Assert.NotNull(client.LastRequest);
            Assert.AreEqual("token", client.LastRequest.BearerTokenOverride);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.AreEqual(3, result.Bytes.Length);
        }

        private static void Run(IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
            }
        }
    }
}
