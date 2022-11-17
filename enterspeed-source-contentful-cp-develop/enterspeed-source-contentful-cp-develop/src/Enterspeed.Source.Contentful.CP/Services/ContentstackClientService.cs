using Contentful.Core;
using System.Net.Http;
using Contentstack.Core;

namespace Enterspeed.Source.Contentful.CP.Services;

public class ContentstackClientService : IContentstackClientService
{
  
    private readonly ContentstackClient _client;

    public ContentstackClientService(IEnterspeedConfigurationService enterspeedConfigurationService)
    {
        var configuration = enterspeedConfigurationService.GetConfiguration();
        var httpClient = new HttpClient();

      
        _client = new ContentstackClient(configuration.ContentstackApiKey, configuration.ContentstackDeliveryToken, configuration.ContentstackEnviroment);
        // _client=new ContentstackClient(
        //     string apiKey,
        //     string deliveryToken,
        //     string environment,
        //     string host = null,
        //     ContentstackRegion region = ContentstackRegion.US,
        //     string version = null)
    }

    public ContentstackClient GetClient()
    {
        return _client;
    }
}