using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace anBlogg.Application.Services.Implementations
{
    public class Properties : IProperties
    {
        public bool NotExistsIn<TSource>(string properties) =>
           !ExistsIn<TSource>(properties);

        public bool ExistsIn<TSource>(string properties)
        {
            var propertiesInfos = GetAllProperties<TSource>();
            var propertiesNames = GetPropertiesNamesFrom(properties);

            foreach (var searchedName in propertiesNames)
                if (PropertyNotExistsIn(propertiesInfos, searchedName))
                    return false;

            return true;
        }

        public IEnumerable<ExpandoObject> ShapeData<T>
           (IEnumerable<T> source, string desirableProperties)
        {
            var expandoObjects = new List<ExpandoObject>();
            var propertiesInfos = GetPropertiesInfos<T>(desirableProperties);

            foreach (T sourceObject in source)
            {
                var shapedData = CreateNewObjectFromProperties(sourceObject, propertiesInfos);
                expandoObjects.Add(shapedData);
            }

            return expandoObjects;
        }

        private ExpandoObject CreateNewObjectFromProperties<TSource>
          (TSource sourceObject, IEnumerable<PropertyInfo> propertiesInfos)
        {
            var shapedData = new ExpandoObject();
            foreach (var propertyInfo in propertiesInfos)
            {
                var propertyValue = propertyInfo.GetValue(sourceObject);
                var castedShapedData = (IDictionary<string, object>)shapedData;
                castedShapedData.Add(propertyInfo.Name, propertyValue);
            }

            return shapedData;
        }

        private List<PropertyInfo> GetPropertiesInfos<TSource>(string desirableProperties)
        {
            PropertyInfo[] propertiesInfo;
            if (string.IsNullOrWhiteSpace(desirableProperties))
                propertiesInfo = GetAllProperties<TSource>();
            else
                propertiesInfo = GetParticularProperties<TSource>(desirableProperties);

            return propertiesInfo.ToList();
        }

        private PropertyInfo[] GetAllProperties<TSource>() =>
           typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private PropertyInfo[] GetParticularProperties<TSource>
           (string unseparatedProperties)
        {
            var propertiesInfo = new List<PropertyInfo>();
            var propertiesNames = GetPropertiesNamesFrom(unseparatedProperties);
            foreach (var propertyName in propertiesNames)
            {
                var propertyInfo = GetSingleProperty<TSource>(propertyName);
                propertiesInfo.Add(propertyInfo);
            }

            return propertiesInfo.ToArray();
        }

        private PropertyInfo GetSingleProperty<TSource>(string propertyName) =>
           typeof(TSource).GetProperty(propertyName,
               BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        private bool PropertyNotExistsIn
            (IEnumerable<PropertyInfo> propertiesInfos, string searchedName) =>
            propertiesInfos.Where(p => p.Name.ToLowerInvariant() == searchedName).Count() == 0;

        public IEnumerable<string> GetPropertiesNamesFrom
            (string unseparatedParameters)
        {
            var parameters = PrepareParameters(unseparatedParameters);
            foreach (var parameter in parameters)
                yield return RemoveAdditionalArgumentsFrom(parameter);
        }

        private string RemoveAdditionalArgumentsFrom(string parameter)
        {
            var indexOfFirstSpace = parameter.IndexOf(' ');
            var noAdditionalArguments = indexOfFirstSpace == -1;
            return noAdditionalArguments ? parameter : parameter.Remove(indexOfFirstSpace);
        }

        public IEnumerable<ArguedProperty> GetPropertiesNamesWithArgumentsFrom
            (string unseparatedParameters)
        {
            var parameters = PrepareParameters(unseparatedParameters);
            foreach (var parameter in parameters)
                yield return SeparateArgumentFromPropertyName(parameter);
        }

        private ArguedProperty SeparateArgumentFromPropertyName(string parameter)
        {
            var splitted = parameter.Split(' ');
            var argument = splitted.Count() > 1 ? splitted[1] : string.Empty;
            return new ArguedProperty(splitted[0], argument);
        }

        private IEnumerable<string> PrepareParameters(string unseparatedParameters)
        {
            var uncutParameters = unseparatedParameters.Split(',');
            foreach (var uncutParameter in uncutParameters)
            {
                var unloweredParameter = uncutParameter.Trim();
                yield return unloweredParameter.ToLowerInvariant();
            }
        }

        public ExpandoObject CreateDynamicResourceFrom<T>(T source)
        {
            var propertiesInfo = GetAllProperties<T>();
            return CreateNewObjectFromProperties(source, propertiesInfo);
        }
    }
}