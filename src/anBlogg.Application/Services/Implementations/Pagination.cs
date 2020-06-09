using anBlogg.Application.Services.Helpers;
using System;
using System.Text.Json;

namespace anBlogg.Application.Services.Implementations
{
    public class Pagination : IPagination
    {
        private readonly IProperties properties;

        public Pagination(IProperties properties) => 
            this.properties = properties;

        public Header CreateHeader<T>(PagedList<T> elements)
        {
            var metadata = CreateMetadata(elements);
            var serializedMetadata = JsonSerializer.Serialize(metadata);

            return new Header("X-Pagination", serializedMetadata);
        }

        private PaginationMetadata CreateMetadata<T>(PagedList<T> elements)
        {
            return new PaginationMetadata
            {
                TotalCount = elements.TotalCount,
                PageSize = elements.PageSize,
                CurrentPage = elements.CurrentPage,
                TotalPages = elements.TotalPages,
            };
        }

        public PagesLinks CreatePagesLinks<T1, T2>(PagedList<T1> elements, 
            T2 parameters, UriResource uriResource)
        {
            if (string.IsNullOrWhiteSpace(uriResource.GetMethodName))
                uriResource.GetMethodName = DeduceGetMethodNameFor(typeof(T1));

            var previousPageLink = elements.HasPrevious ?
                CreateResourceUri(parameters, uriResource, ResourceUriType.PreviousPage) 
                : null;

            var nextPageLink = elements.HasNext ?
               CreateResourceUri(parameters, uriResource, ResourceUriType.NextPage) 
               : null;

            return new PagesLinks(previousPageLink, nextPageLink);
        }

        private string DeduceGetMethodNameFor(Type element)
        {
            var typeName = element.Name;
            return "Get" + typeName + "s";
        }

        public string CreateResourceUri<T>
            (T source, UriResource uriResource, ResourceUriType type)
        {
            var resource = properties.CreateDynamicResourceFrom(source);
            ((dynamic)resource).PageNumber += GetValueToAddDependingOn(type);

            return uriResource.UrlHelper.Link(uriResource.GetMethodName, resource);
        }

        private int GetValueToAddDependingOn(ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => -1,
                ResourceUriType.NextPage => 1,
                ResourceUriType.Current => 0,
                _ => 0
            };
        }
    }
}