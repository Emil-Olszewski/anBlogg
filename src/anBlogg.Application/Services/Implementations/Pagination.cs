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
        private IUrlHelper url;
        private string previousPageLink;
        private string nextPageLink;

        public Pagination(IProperties properties)
        {
            this.properties = properties;
        }

        public Header CreateHeader<T>(PagedList<T> elements,
            IResourceParameters parameters, IUrlHelper url)
        {
            this.parameters = parameters;
            this.url = url;

            CreatePageLinks(elements);

            var paginationMetadata = CreateMetadata
                (elements, previousPageLink, nextPageLink);

            var serializedMetadata = JsonSerializer.Serialize(paginationMetadata);

            return new Header("X-Pagination", serializedMetadata);
        }

        private void CreatePageLinks<T>(PagedList<T> elements)
        {
            var methodName = GetExpectedGetMethodNameFor(elements[0]);
            previousPageLink = elements.HasPrevious ?
                CreateResourceUri(ResourceUriType.PreviousPage, methodName) : null;

            nextPageLink = elements.HasNext ?
               CreateResourceUri(ResourceUriType.NextPage, methodName) : null;
        }

        private string GetExpectedGetMethodNameFor(object element)
        {
            var typeNameWithNamespaces = element.GetType().ToString();
            var splitted = typeNameWithNamespaces.Split('.');
            var typeName = splitted[splitted.Length - 1];
            return "Get" + typeName + "s";
        }

        private string CreateResourceUri(ResourceUriType type, string methodName)
        {
            var resource = properties.CreateDynamicResourceFrom(parameters);
            ((dynamic)resource).PageNumber += GetValueToAddDependingOn(type);

            return url.Link(methodName, resource);
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