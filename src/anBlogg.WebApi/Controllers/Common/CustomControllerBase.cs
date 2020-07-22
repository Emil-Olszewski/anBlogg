using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Common;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.WebApi.Helpers;
using anBlogg.WebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace anBlogg.WebApi.Controllers.Common
{
    public abstract class CustomControllerBase : ControllerBase
    {
        protected readonly IValidator validator;
        protected readonly IMapper mapper;
        protected readonly IBlogRepository blogRepository;
        protected readonly IPropertyMappingService mappingService;
        protected readonly IProperties properties;
        protected readonly IPagination pagination;

        public CustomControllerBase(IMapper mapper, IBlogRepository blogRepository,
            IValidator validator, IProperties properties, IPagination pagination)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
            this.validator = validator;
            this.properties = properties;
            this.pagination = pagination;
        }

        protected void AddPaginationHeader<T>(PagedList<T> source)
        {
            var header = pagination.CreateHeader(source);
            Response.Headers.Add(header.Name, header.Value);
            Response.Headers.Add("Access-Control-Expose-Headers", header.Name);
        }

        protected bool IncludeLinks(MediaTypeHeaderValue mediaType)
        {
            return mediaType != null && mediaType.SubTypeWithoutSuffix.EndsWith
                ("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        protected bool IncludeLinks(string mediaType)
        {
            var parsedMediaType = GetMediaType(mediaType);
            return IncludeLinks(parsedMediaType);
        }

        protected MediaTypeHeaderValue GetMediaType(string mediaType)
        {
            MediaTypeHeaderValue.TryParse(mediaType,
                out MediaTypeHeaderValue parsedMediaType);

            return parsedMediaType;
        }

        protected dynamic GetCollectionWithLinks<T>(PagedList<T> resources,
            IEnumerable<ExpandoObject> shapedResources, IResourceParameters parameters)
            where T : AuditableEntity
        {
            var ids = GetIds(resources).ToList();

            return new
            {
                value = GetLinkedResources(shapedResources, ids, parameters.Fields),
                links = CreateLinksForCollection(resources, parameters)
            };
        }

        protected IEnumerable<IDictionary<string, object>> GetLinkedResources
            (IEnumerable<ExpandoObject> shapedResources, List<IIdsSet> ids, string fields)
        {
            var counter = 0;

            return shapedResources.Select(TransformIntoDictionary);

            IDictionary<string, object> TransformIntoDictionary(ExpandoObject resource)
            {
                var resourceLinks = CreateLinksForSingleResource(ids[counter], fields);
                var resourceAsDictionary = resource as IDictionary<string, object>;
                resourceAsDictionary.Add("links", resourceLinks);

                counter++;
                return resourceAsDictionary;
            }
        }

        protected IDictionary<string, object> GetLinkedResource
            (ExpandoObject shapedResource, IIdsSet idsSet, string fields = "")
        {
            var links = CreateLinksForSingleResource(idsSet, fields);
            var linkedResource = shapedResource as IDictionary<string, object>;
            linkedResource.Add("links", links);
            return linkedResource;
        }

        protected IEnumerable<LinkDto> CreateLinksForCollection<T>
            (PagedList<T> resources, IResourceParameters parameters)
            where T : AuditableEntity
        {
            var links = new List<LinkDto>();
            var resourceUriHelper = new ResourceUriHelper(Url);
            var resourceUri = pagination.CreateResourceUri
                (parameters, resourceUriHelper, ResourceUriType.Current);

            links.Add(new LinkDto(resourceUri, "self", "GET"));

            var pagesLinks = pagination.CreatePagesLinks(resources, parameters, resourceUriHelper);

            if (pagesLinks.HasPrevious)
                links.Add(new LinkDto(pagesLinks.Previous, "previousPage", "GET"));

            if (pagesLinks.HasNext)
                links.Add(new LinkDto(pagesLinks.Next, "nextPage", "GET"));

            return links;
        }

        protected bool IsUserIdNotEqualTo(Guid id)
        {
            return GetParsedUserId() != id ? true : false;
        }

        protected Guid GetParsedUserId()
        {
            return Guid.Parse(GetUserId());
        }

        protected string GetUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }

        protected abstract IEnumerable<LinkDto> CreateLinksForSingleResource
            (IIdsSet ids, string fields);

        protected abstract IEnumerable<IIdsSet> GetIds<T>(PagedList<T> resources)
            where T : AuditableEntity;
    }
}