using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core.Models.Management;
using Contentstack.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Enterspeed.Source.Contentful.CP.Constants;
using Enterspeed.Source.Contentful.CP.Exceptions;
using Enterspeed.Source.Contentful.CP.Handlers;
using Enterspeed.Source.Contentful.CP.Models;
using Locale = Contentstack.Management.Core.Models.Locale;

namespace Enterspeed.Source.Contentful.CP;

public class ContentstackWebhooks
{
    private readonly IEnumerable<IEnterspeedEventHandler> _enterspeedEventHandlers;

    public ContentstackWebhooks(IEnumerable<IEnterspeedEventHandler> enterspeedEventHandlers)
    {
        _enterspeedEventHandlers = enterspeedEventHandlers;
    }

    [FunctionName("ContentstackWebhooks")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1")] HttpRequest req, ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var requestAssets = JsonConvert.DeserializeObject<AssetLibrary>(requestBody);
        var requestLocale = JsonConvert.DeserializeObject<Locale>(requestBody);
        

       



        /* TODO missing
            preview
            Empty fields are not available through post data or delivery api
            Check all data types from Contentful
                Rich text
            
            event types
                ContentManagement.Entry.publish         - Done
                ContentManagement.Entry.delete          - Done
                ContentManagement.Entry.unpublish
                ContentManagement.Entry.save
                ContentManagement.Entry.auto_save
                ContentManagement.Entry.create
                ContentManagement.Entry.archive
                ContentManagement.Entry.unarchive

                ContentManagement.Asset.publish         - Done
                ContentManagement.Asset.delete          - Done
                ContentManagement.Asset.unpublish
                ContentManagement.Asset.archive
                ContentManagement.Asset.unarchive
                ContentManagement.Asset.save
                ContentManagement.Asset.auto_save
                ContentManagement.Asset.create

                ContentManagement.Seed                  - Done
        */

        if (!req.Headers.TryGetValue(WebhooksConstants.EventHeaderKey, out var contentfulEventName))
        {
            return new BadRequestObjectResult($"no header with name '{WebhooksConstants.EventHeaderKey}', was provided");
        }

        if (contentfulEventName.Count > 1)
        {
            return new BadRequestObjectResult($"Multiple values provided for header '{WebhooksConstants.EventHeaderKey}', only one is allowed");
        }

        var enterspeedEventHandler = _enterspeedEventHandlers.FirstOrDefault(x => x.CanHandle(requestAssets,requestLocale, contentfulEventName));
        
        if (enterspeedEventHandler == null)
        {
            return new BadRequestObjectResult($"no handler found for event '{contentfulEventName}'");
        }

        try
        {
            enterspeedEventHandler.Handle(requestAssets,requestLocale);
        }
        catch (EventHandlerException exception)
        {
            return new UnprocessableEntityObjectResult(exception.Message);
        }

        return new OkObjectResult("OK");
    }
}