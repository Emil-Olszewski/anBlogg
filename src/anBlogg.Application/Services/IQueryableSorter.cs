using anBlogg.Application.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.Application.Services
{
    public interface IQueryableSorter
    {
        IQueryable<T> ApplySort<T>(IQueryable<T> source, string parameters,
            Dictionary<string, PropertyMappingValue> mappingDictionary);
    }
}