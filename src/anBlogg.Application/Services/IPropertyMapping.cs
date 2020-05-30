using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;

namespace anBlogg.Application.Services
{
    public interface IPropertyMapping
    {
        Dictionary<string, PropertyMappingValue> MappingDictionary { get; }
    }
}