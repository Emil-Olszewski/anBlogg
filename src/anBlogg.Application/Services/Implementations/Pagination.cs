using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace anBlogg.Application.Services.Implementations
{
    public class Pagination : IPagination
    {
        private readonly IProperties properties;
        private IResourceParametersBase parameters;
        private IUrlHelper url;
        private string previousPageLink;
        private string nextPageLink;

        public Pagination(IProperties properties)
        {
            this.properties = properties;
        }

        public Header CreatePaginationHeader<T>(PagedList<T> elements, IResourceParametersBase parameters, IUrlHelper url)
        {
            this.parameters = parameters;
            this.url = url;

            CreatePageLinks(elements);

            var paginationMetadata = CreatePaginationMetadata(elements, previousPageLink, nextPageLink);

            var serializedMetadata = JsonSerializer.Serialize(paginationMetadata);

            return new Header("X-Pagination", serializedMetadata);
        }

        private void CreatePageLinks<T>(PagedList<T> elements)
        {
            previousPageLink = elements.HasPrevious ?
                CreateResourceUri(ResourceUriType.PreviousPage) : null;

            nextPageLink = elements.HasNext ?
               CreateResourceUri(ResourceUriType.NextPage) : null;

        }

        private string CreateResourceUri(ResourceUriType type)
        {
            var resource = properties.CreateDynamicResourceFrom(parameters);
            ((dynamic)resource).PageNumber += GetValueToAddDependingOn(type);

            return url.Link("GetPosts", resource);
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

        private dynamic CreatePaginationMetadata<T>(PagedList<T> elements, 
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
