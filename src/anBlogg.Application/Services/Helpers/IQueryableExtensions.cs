using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace anBlogg.Application.Services.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderParameters,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var orderBy = PrepareOrderByString(orderParameters, mappingDictionary);
            return source.OrderBy(orderBy);
        }

        private static string PrepareOrderByString(string orderParameters, 
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var orderBy = string.Empty;
            var splittedParameters = orderParameters.Split(',');

            foreach (var parameter in splittedParameters)
                orderBy = BuiltOrderByString(orderBy, parameter, mappingDictionary);

            return orderBy;
        }

        private static string BuiltOrderByString(string orderBy, string orderParameter,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var trimmedParameter = orderParameter.Trim();
            var orderDescending = trimmedParameter.EndsWith("desc");
            var propertyName = ExtractPropertyName(trimmedParameter);
            var propertyMappingValue = mappingDictionary[propertyName];

            foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
            {
                if (propertyMappingValue.Revert)
                    orderDescending = !orderDescending;

                orderBy = Merge(orderBy, destinationProperty, orderDescending);
            }

            return orderBy;
        }

        private static string ExtractPropertyName(string trimmedOrder)
        {
            var index = trimmedOrder.IndexOf(" ");
            return index == -1 ? trimmedOrder : trimmedOrder.Remove(index);
        }

        private static string Merge(string orderByString, 
            string destinationProperty, bool orderDescending)
        {
            var comma = string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ";
            var order = (orderDescending ? " descending" : " ascending");
            return orderByString + comma + destinationProperty + order;
        }
    }
}

