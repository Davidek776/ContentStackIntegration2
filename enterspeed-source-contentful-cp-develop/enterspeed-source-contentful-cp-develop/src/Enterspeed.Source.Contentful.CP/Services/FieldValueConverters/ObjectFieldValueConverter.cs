using System.Collections.Generic;
using Contentful.Core.Models.Management;
using Enterspeed.Source.Contentful.CP.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Contentful.CP.Services.FieldValueConverters;

public class ObjectFieldValueConverter : IEnterspeedFieldValueConverter
{
    private readonly IEntityIdentityService _entityIdentityService;

    public ObjectFieldValueConverter(IEntityIdentityService entityIdentityService)
    {
        _entityIdentityService = entityIdentityService;
    }

    public bool IsConverter(ContentfulField field)
    {
        return field.Type == typeof(ContentfulResource);
    }

    public IEnterspeedProperty Convert(ContentfulField field, Locale locale)
    {
        var value = ((ContentfulObjectField)field).GetValue();
        var properties = new Dictionary<string, IEnterspeedProperty>
        {
            ["id"] = new StringEnterspeedProperty("id", _entityIdentityService.GetId(value.SystemProperties.Id, locale)),
            ["type"] = new StringEnterspeedProperty("type", value.SystemProperties.Type),
            ["linkType"] = new StringEnterspeedProperty("linkType", value.SystemProperties.LinkType)
        };

        return new ObjectEnterspeedProperty(field.Name, properties);
    }
}