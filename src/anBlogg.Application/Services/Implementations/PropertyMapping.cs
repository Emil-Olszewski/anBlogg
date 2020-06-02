using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;

namespace anBlogg.Application.Services.Implementations
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary) =>
            MappingDictionary = mappingDictionary;
    }
}