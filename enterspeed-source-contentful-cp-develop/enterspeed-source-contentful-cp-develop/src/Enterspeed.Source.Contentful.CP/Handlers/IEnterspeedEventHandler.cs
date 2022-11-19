using Contentful.Core.Models;
using Contentstack.Core.Models;
using Contentstack.Management.Core.Models;
using Asset = Contentstack.Core.Models.Asset;

namespace Enterspeed.Source.Contentful.CP.Handlers;

public interface IEnterspeedEventHandler
{
    bool CanHandle(AssetLibrary assetResource, Locale assetLocale, string eventType);
    void Handle(AssetLibrary resource, Locale assetLocale);
}