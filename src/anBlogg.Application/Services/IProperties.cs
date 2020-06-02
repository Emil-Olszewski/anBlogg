using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;
using System.Dynamic;

public interface IProperties
{
    bool NotExistsIn<TSource>(string properties);

    bool ExistsIn<TSource>(string properties);

    IEnumerable<ExpandoObject> ShapeData<TSource>
        (IEnumerable<TSource> source, string desirableProperties);

    IEnumerable<string> GetPropertiesNamesFrom(string unseparatedParameters);

    IEnumerable<PropertyParameter> GetPropertiesNameWithArgumentsFrom
        (string unseparatedParameters);

    ExpandoObject CreateDynamicResourceFrom<T>(T source);
}