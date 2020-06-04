using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace anBlogg.Application.Services.Implementations
{
    public class Pagination : IPagination
    {
        private readonly IProperties properties;
        private IResourceParameters parameters;
        private UriResource uriResource;
        private string previousPageLink;
        private string nextPageLink;

        public Pagination(IProperties properties) => 
            this.properties = properties;

        public Header CreateHeader<T>(PagedList<T> elements,
            IResourceParameters parameters, UriResource uriResource)
        {
            this.parameters = parameters;
            this.uriResource = uriResource;

            CreatePageLinks(elements);

            var paginationMetadata = CreateMetadata
                (elements, previousPageLink, nextPageLink);

            var serializedMetadata = JsonSerializer.Serialize(paginationMetadata);

            return new Header("X-Pagination", serializedMetadata);
        }

        private void CreatePageLinks<T>(PagedList<T> elements)
        {
            var methodName = NeedsToDeduceName() ? 
                DeduceGetMethodNameFor(elements[0]) : uriResource.GetMethodName;

            previousPageLink = elements.HasPrevious ?
                CreateResourceUri(ResourceUriType.PreviousPage, methodName) : null;

            nextPageLink = elements.HasNext ?
               CreateResourceUri(ResourceUriType.NextPage, methodName) : null;
        }

        private bool NeedsToDeduceName() =>
            string.IsNullOrWhiteSpace(uriResource.GetMethodName);

        private string DeduceGetMethodNameFor(object element)
        {
            var typeNameWithNamespaces = element.GetType().ToString();
            var splitted = typeNameWithNamespaces.Split('.');
            var typeName = splitted[^1];
            return "Get" + typeName + "s";
        }

        private string CreateResourceUri(ResourceUriType type, string methodName)
        {
            var resource = properties.CreateDynamicResourceFrom(parameters);
            ((dynamic)resource).PageNumber += GetValueToAddDependingOn(type);

            return uriResource.UrlHelper.Link(methodName, resource);
        }

        private int GetValueToAddDependingOn(ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => -1,
                ResourceUriType.NextPage => 1,
                _ => 0
            };
        }

        private dynamic CreateMetadata<T>(PagedList<T> elements,
            string previousPageLink, string nextPageLink)
        {
            return new
            {
                totalCount = elements.TotalCount,
                pageSize = elements.PageSize,
                currentPage = elements.CurrentPage,
                totalPages = elements.TotalPages,
                previousPageLink,
                nextPageLink
            };
        }
    }
}