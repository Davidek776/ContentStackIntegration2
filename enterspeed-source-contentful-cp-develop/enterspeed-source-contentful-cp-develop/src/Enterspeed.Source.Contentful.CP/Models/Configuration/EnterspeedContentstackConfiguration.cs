using Enterspeed.Source.Sdk.Configuration;

namespace Enterspeed.Source.Contentful.CP.Models.Configuration;

public class EnterspeedContententstackConfiguration : EnterspeedConfiguration
{
    public string ContentstackApiKey { get; set; }
    public string ContentstackDeliveryToken { get; set; }
    public string ContentstackEnviroment { get; set; }
}