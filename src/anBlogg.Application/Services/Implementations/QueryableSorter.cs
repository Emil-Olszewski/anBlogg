using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace anBlogg.Application.Services.Implementations
{
    public class QueryableSorter : IQueryableSorter
    {
        private readonly IProperties properties;

        public QueryableSorter(IProperties properties) =>
            this.properties = properties;

        public IQueryable<T> ApplySort<T>(IQueryable<T> source, string parameters,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var orderInstruction =
                TransformParametersIntoUnderstandableString(parameters, mappingDictionary);

            return source.OrderBy(orderInstruction);
        }

        private string TransformParametersIntoUnderstandableString
            (string unseparatedParameters, 
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var orderInstruction = string.Empty;
            var arguedProperties = properties
                .GetPropertiesNamesWithArgumentsFrom(unseparatedParameters);

            foreach (var property in arguedProperties)
                orderInstruction = BuiltOrderInstruction
                    (orderInstruction, property, mappingDictionary);

            return orderInstruction;
        }

        private static string BuiltOrderInstruction(string orderInstruction, ArguedProperty parameter,
           Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var isOrderDescending = parameter.Argument == "desc";
            var propertyMappingValue = mappingDictionary[parameter.PropertyName];

            foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
            {
                if (propertyMappingValue.Revert)
                    isOrderDescending = !isOrderDescending;

                orderInstruction = Merge(orderInstruction, destinationProperty, isOrderDescending);
            }

            return orderInstruction;
        }

        private static string Merge(string orderInstruction,
            string destinationProperty, bool orderDescending)
        {
            var commaOrNothing = string.IsNullOrWhiteSpace(orderInstruction) ? string.Empty : ", ";
            var order = (orderDescending ? " descending" : " ascending");
            return orderInstruction + commaOrNothing + destinationProperty + order;
        }
    }
}