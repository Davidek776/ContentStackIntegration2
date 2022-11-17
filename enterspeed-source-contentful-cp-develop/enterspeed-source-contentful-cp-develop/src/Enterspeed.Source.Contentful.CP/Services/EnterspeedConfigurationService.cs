using Enterspeed.Source.Contentful.CP.Models.Configuration;
using System;

namespace Enterspeed.Source.Contentful.CP.Services;

public class EnterspeedConfigurationService : IEnterspeedConfigurationService
{
    public EnterspeedContententstackConfiguration GetConfiguration()
    {
        return new EnterspeedContententstackConfiguration
        {
            ApiKey = Environment.GetEnvironmentVariable("Enterspeed.ApiKey"),
            BaseUrl = Environment.GetEnvironmentVariable("Enterspeed.BaseUrl"),
            ContentstackApiKey = Environment.GetEnvironmentVariable("Contentstack.ApiKey"),
            ContentstackDeliveryToken  = Environment.GetEnvironmentVariable("Contentstack.DeliveryToekn"),
            ContentstackEnviroment = Environment.GetEnvironmentVariable("Contentstack.Environment")
        };
    }
}