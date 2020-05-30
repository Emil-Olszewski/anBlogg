using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;

namespace anBlogg.Application.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}