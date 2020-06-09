using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace anBlogg.Application.Services.Implementations
{
    public class Properties : IProperties
    {
        public bool NotExistsIn<T>(string properties) =>
           !ExistsIn<T>(properties);

        public bool ExistsIn<T>(string properties)
        {
            var propertiesInfos = GetAllProperties<T>();
            var propertiesNames = GetPropertiesNamesFrom(properties);

            foreach (var searchedName in propertiesNames)
                if (PropertyNotExistsIn(propertiesInfos, searchedName))
                    return false;

            return true;
        }

        public ExpandoObject ShapeSingleData<T>
           (T source, string desirableProperties)
        {
            var propertiesInfos = GetPropertiesInfos<T>(desirableProperties);
            return CreateNewObjectFromProperties(source, propertiesInfos);
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

        private ExpandoObject CreateNewObjectFromProperties<T>
          (T sourceObject, IEnumerable<PropertyInfo> propertiesInfos)
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

        private List<PropertyInfo> GetPropertiesInfos<T>(string desirableProperties)
        {
            PropertyInfo[] propertiesInfo;
            if (string.IsNullOrWhiteSpace(desirableProperties))
                propertiesInfo = GetAllProperties<T>();
            else
                propertiesInfo = GetParticularProperties<T>(desirableProperties);

            return propertiesInfo.ToList();
        }

        private PropertyInfo[] GetAllProperties<T>() =>
           typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private PropertyInfo[] GetParticularProperties<T>
           (string unseparatedProperties)
        {
            var propertiesInfo = new List<PropertyInfo>();
            var propertiesNames = GetPropertiesNamesFrom(unseparatedProperties);
            foreach (var propertyName in propertiesNames)
            {
                var propertyInfo = GetSingleProperty<T>(propertyName);
                propertiesInfo.Add(propertyInfo);
            }

            return propertiesInfo.ToArray();
        }

        private PropertyInfo GetSingleProperty<T>(string propertyName) =>
           typeof(T).GetProperty(propertyName,
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