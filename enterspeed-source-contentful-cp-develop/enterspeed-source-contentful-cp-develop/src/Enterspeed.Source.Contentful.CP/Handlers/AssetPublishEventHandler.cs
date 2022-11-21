using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentstack.Core.Models;
using Contentstack.Management.Core.Models;
using Enterspeed.Source.Contentful.CP.Constants;
using Enterspeed.Source.Contentful.CP.Exceptions;
using Enterspeed.Source.Contentful.CP.Models;
using Enterspeed.Source.Contentful.CP.Services;
using Enterspeed.Source.Sdk.Api.Services;
using Asset = Contentful.Core.Models.Asset;

namespace Enterspeed.Source.Contentful.CP.Handlers;

public class AssetPublishEventHandler : IEnterspeedEventHandler
{
    private readonly IContentstackClientService _contentfulClientService;
    private readonly IEnterspeedPropertyService _enterspeedPropertyService;
    private readonly IEntityIdentityService _entityIdentityService;
    private readonly IEnterspeedIngestService _enterspeedIngestService;

    public AssetPublishEventHandler(IContentstackClientService contentfulClientService, IEnterspeedPropertyService enterspeedPropertyService, IEntityIdentityService entityIdentityService, IEnterspeedIngestService enterspeedIngestService)
    {
        _contentfulClientService = contentfulClientService;
        _enterspeedPropertyService = enterspeedPropertyService;
        _entityIdentityService = entityIdentityService;
        _enterspeedIngestService = enterspeedIngestService;
    }

    public bool CanHandle(AssetLibrary assetResource, string eventType)
    {
        // to do conditions
        return eventType == WebhooksConstants.Events.AssetPublish;
    }

    public async void Handle(AssetLibrary assetResource,Locale locale)
    {
        var client = _contentfulClientService.GetClient();
        

        // Don't know how to continue with passing locale argument to queryBuilder
            var queryBuilder = QueryBuilder<Asset>.New.LocaleIs(locale);
          

            var entity = new EnterspeedEntity(assetResource, locale, _enterspeedPropertyService, _entityIdentityService);

            var saveResponse = _enterspeedIngestService.Save(entity);
            if (!saveResponse.Success)
            {
                var message = saveResponse.Exception != null
                    ? saveResponse.Exception.Message
                    : saveResponse.Message;
                throw new EventHandlerException($"Failed ingesting entity ({entity.Id}). Message: {message}");
            }
        
    }
}