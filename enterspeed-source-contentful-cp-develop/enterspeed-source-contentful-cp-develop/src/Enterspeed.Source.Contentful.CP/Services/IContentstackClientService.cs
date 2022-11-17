using Contentful.Core;
using Contentstack.Core;


namespace Enterspeed.Source.Contentful.CP.Services;

public interface IContentstackClientService
{
    ContentstackClient GetClient();
}