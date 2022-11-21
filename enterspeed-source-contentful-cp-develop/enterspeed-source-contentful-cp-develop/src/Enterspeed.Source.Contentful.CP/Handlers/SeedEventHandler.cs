using System.Collections.Generic;
using System.Linq;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentstack.Core;
using Contentstack.Core.Models;
using Contentstack.Management.Core.Models;
using Enterspeed.Source.Contentful.CP.Constants;
using Enterspeed.Source.Contentful.CP.Models;
using Enterspeed.Source.Contentful.CP.Services;
using Asset = Contentstack.Management.Core.Models.Asset;

namespace Enterspeed.Source.Contentful.CP.Handlers;

public class SeedEventHandler : IEnterspeedEventHandler
{
    private readonly IContentstackClientService _contentstackClientService;
    private readonly IEnumerable<IEnterspeedEventHandler> _enterspeedEventHandlers;

    public SeedEventHandler(IContentstackClientService contentfulClientService, IEnumerable<IEnterspeedEventHandler> enterspeedEventHandlers)
    {
        _contentstackClientService = contentfulClientService;
        _enterspeedEventHandlers = enterspeedEventHandlers;
    }

    // As you can see my idea was to seperate Assets, Entries and Locale because they were not inside ContentstackClient 
    public bool CanHandle(AssetLibrary assetResource, Locale localeResource, string eventType)
    {
        return eventType == WebhooksConstants.Events.Seed;
    }

    public void Handle(AssetLibrary assetResource,Locale localeResource)
    {
        var client = _contentstackClientService.GetClient();
        // Source
        // HandleEntries(client);
        HandleAssets(assetResource, localeResource);
    }

    private async void HandleEntries(ContentfulClient client)
    {
        var entryQueryBuilder = QueryBuilder<ContentfulResource>.New;
        var entries = await client.GetEntries(entryQueryBuilder);
    
        var entryPublishEventHandler = _enterspeedEventHandlers.First(x => x.GetType() == typeof(EntryPublishEventHandler));
    
        foreach (var entry in entries)
        {
            // entryPublishEventHandler.Handle(entry);
        }
    }

    // I have focused only on Assets for now
    private async void HandleAssets(AssetLibrary assetResource,Locale localeResource)
    {
        var assetQueryBuilder = QueryBuilder<Asset>.New; //this Query should be also substitude
        var assets = await assetResource.FetchAll();

        var assetPublishEventHandler = _enterspeedEventHandlers.First(x => x.GetType() == typeof(AssetPublishEventHandler));

        // foreach (var asset in assets)
        // {
            assetPublishEventHandler.Handle(assetResource,localeResource);
        // }
    }
}