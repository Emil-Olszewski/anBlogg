using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.Application.Services.Implementations
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> postPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "Date", new PropertyMappingValue(new List<string>() { "Created" }, true ) },
                { "Title", new PropertyMappingValue(new List<string>() { "Title" } ) },
                { "Score", new PropertyMappingValue(new List<string>() { "Score" }, true ) }
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService() => 
            propertyMappings.Add(new PropertyMapping<IPostOutputDto, Post>(postPropertyMapping));

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
                return matchingMapping.First().MappingDictionary;

            throw new Exception("Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)}, {typeof(TDestination)}>");
        }
    }
}
