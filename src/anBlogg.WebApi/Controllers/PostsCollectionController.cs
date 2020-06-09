using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : CustomControllerBase
    {
        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository,
           IValidator validator, IProperties properties, IPagination pagination)
           : base(mapper, blogRepository, validator, properties, pagination)
        {
        }

        [HttpGet(Name = "GetPosts")]
        public ActionResult<IEnumerable<IPostOutputDto>> GetPosts(
            [FromHeader(Name = "Accept")] string mediaType,
            [FromQuery] PostResourceParameters parameters)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
                return BadRequest();

            if (CantValidate(parameters))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPosts(parameters);

            AddHeader(postsFromRepo);

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var links = new List<LinkDto>();

            if (includeLinks)
                links = CreateLinksForPosts(postsFromRepo, parameters).ToList();

            var primaryMediaType = includeLinks ? parsedMediaType.SubTypeWithoutSuffix
                .Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8) :
                parsedMediaType.SubTypeWithoutSuffix;

            if (parsedMediaType.MediaType == "application/vnd.anblogg.hateoas+json")
            {
                var infos = mappedPosts.Select(GetIds).ToList();

                static dynamic GetIds(PostOutputDto post) =>
                    new { post.Id, AuthorId = post.Author.Id };

                var counter = 0;

                var shapedPostsWithLinks = shapedPosts.Select(TransformIntoDictionary);

                IDictionary<string, object> TransformIntoDictionary(ExpandoObject post)
                {
                    var postLinks = CreateLinksForPost(infos[counter].AuthorId, infos[counter].Id, null);
                    var postAsDictionary = post as IDictionary<string, object>;
                    postAsDictionary.Add("links", postLinks);
                    counter++;
                    return postAsDictionary;
                }

                var linkedCollectionResource = new
                {
                    value = shapedPostsWithLinks,
                    links
                };

                return Ok(linkedCollectionResource);
            }

            return Ok(shapedPosts);
        }

        protected bool CantValidate(IPostResourceParameters parameters)
        {
            return (validator.DontMatchRules(parameters) ||
                validator.OrderIsInvalid<Post, IPostOutputDto>(parameters.OrderBy) ||
                validator.FieldsAreInvalid<IPostOutputDto>(parameters.Fields));
        }

        private IEnumerable<LinkDto> CreateLinksForPost(Guid authorId, Guid postId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(new LinkDto(Url.Link("GetPost",
                    new { authorId, postId }), "self", "GET"));
            else
                links.Add(new LinkDto(Url.Link("GetPost",
                new { authorId, postId, fields }), "self", "GET"));

            links.Add(new LinkDto(Url.Link("CreatePost",
                new { authorId }), "create_post", "POST"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForPosts
            (PagedList<Post> posts, PostResourceParameters parameters)
        {
            var links = new List<LinkDto>();
            var uriResource = new UriResource(Url, "GetPosts");
            var resourceUri = pagination.CreateResourceUri(parameters, uriResource, ResourceUriType.Current);

            links.Add(new LinkDto(resourceUri, "self", "GET"));

            var pagesLinks = pagination.CreatePagesLinks(posts, parameters, uriResource);

            if (pagesLinks.HasPrevious)
                links.Add(new LinkDto(pagesLinks.Previous, "previousPage", "GET"));

            if (pagesLinks.HasNext)
                links.Add(new LinkDto(pagesLinks.Next, "nextPage", "GET"));

            return links;
        }
    }
}