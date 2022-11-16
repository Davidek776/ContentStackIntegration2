using Contentful.Core;
using System.Net.Http;
using Contentstack.Core;

namespace Enterspeed.Source.Contentful.CP.Services;

public class ContentfulClientService : IContentfulClientService
{
    private readonly ContentfulClient _client;
    // private readonly ContentstackClient _client;

    public ContentfulClientService(IEnterspeedConfigurationService enterspeedConfigurationService)
    {
        var configuration = enterspeedConfigurationService.GetConfiguration();
        var httpClient = new HttpClient();

        _client = new ContentfulClient(configuration.ContentfulDeliveryApiKey, configuration.ContentfulPreviewApiKey, spaceId: configuration.ContentfulSpaceId);
    }

    public ContentfulClient GetClient()
    {
        return _client;
    }
}