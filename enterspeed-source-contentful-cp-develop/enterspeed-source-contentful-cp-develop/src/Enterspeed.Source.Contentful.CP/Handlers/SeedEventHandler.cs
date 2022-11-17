using System.Collections.Generic;
using System.Linq;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentstack.Core;
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

    public bool CanHandle(S resource, string eventType)
    {
        return eventType == WebhooksConstants.Events.Seed;
    }

    public void Handle(Contentstack.Management.Core.Models.Asset resource)
    {
        var client = _contentstackClientService.GetClient();
        // Source
        // HandleEntries(client);
        HandleAssets(client);
    }

    private async void HandleEntries(ContentfulClient client)
    {
        var entryQueryBuilder = QueryBuilder<ContentfulResource>.New;
        var entries = await client.GetEntries(entryQueryBuilder);
    
        var entryPublishEventHandler = _enterspeedEventHandlers.First(x => x.GetType() == typeof(EntryPublishEventHandler));
    
        foreach (var entry in entries)
        {
            entryPublishEventHandler.Handle(entry);
        }
    }

    private async void HandleAssets(ContentstackClient client)
    {
        var assetQueryBuilder = QueryBuilder<Asset>.New;
        var assets = await client.get(assetQueryBuilder);

        var assetPublishEventHandler = _enterspeedEventHandlers.First(x => x.GetType() == typeof(AssetPublishEventHandler));

        foreach (var asset in assets)
        {
            assetPublishEventHandler.Handle(asset);
        }
    }
}