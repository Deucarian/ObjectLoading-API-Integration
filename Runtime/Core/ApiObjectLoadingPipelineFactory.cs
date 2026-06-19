using System;
using Deucarian.API.Core;
using Deucarian.ObjectLoading;

namespace Deucarian.ObjectLoading.APIIntegration
{
    public static class ApiObjectLoadingPipelineFactory
    {
        public static ObjectLoadingPipeline Create(IApiClient apiClient)
        {
            return Create(
                apiClient,
                new DirectUrlSourceResolver(),
                new SourceAssetBundleContentLoader(),
                new AssetBundleObjectInstantiator(),
                new DefaultObjectDiagnostics());
        }

        public static ObjectLoadingPipeline Create(IApiClient apiClient,
                                                   IObjectSourceResolver sourceResolver,
                                                   IObjectSourceContentLoader fallbackContentLoader,
                                                   IObjectInstantiator instantiator,
                                                   IObjectDiagnostics diagnostics)
        {
            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient));
            }

            return new ObjectLoadingPipeline(
                sourceResolver ?? new DirectUrlSourceResolver(),
                new ApiAssetBundleSourceContentLoader(
                    apiClient,
                    fallbackContentLoader ?? new SourceAssetBundleContentLoader()),
                instantiator ?? new AssetBundleObjectInstantiator(),
                diagnostics ?? new DefaultObjectDiagnostics());
        }
    }
}
