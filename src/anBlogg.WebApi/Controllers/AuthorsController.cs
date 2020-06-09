using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace anBlogg.WebApi.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : CustomControllerBase
    {
        public AuthorsController(IMapper mapper, IBlogRepository blogRepository,
            IValidator validator, IProperties properties, IPagination pagination)
            : base(mapper, blogRepository, validator, properties, pagination)
        {
        }

        [HttpGet(Name = "GetAuthors")]
        public ActionResult<IEnumerable<AuthorOutputDto>> GetAllAuthors(
            [FromHeader(Name = "Accept")] string mediaType,
            [FromQuery] BasicResourceParameters parameters)
        {
            var parsedMediaType = GetMediaType(mediaType);
            var fullMedia = FullMedia(parsedMediaType);

            if (parsedMediaType is null || CantValidate(parameters, fullMedia))
                return BadRequest();

            var authorsFromRepo = blogRepository.GetAllAuthors(parameters);

            AddHeader(authorsFromRepo);

            var shapedAuthors = GetShapedAuthors
                (authorsFromRepo, parameters.Fields, fullMedia);

            if (IncludeLinks(parsedMediaType))
            {
                var linkedAuthors = GetLinkedAuthorsWithLinks
                    (authorsFromRepo, shapedAuthors, parameters);

                return Ok(linkedAuthors);
            }

            return Ok(shapedAuthors);
        }

        private bool CantValidate(IResourceParameters parameters, bool fullMedia)
        {
            return (validator.DontMatchRules(parameters, ModelState) ||
                OrderIsInvalid(parameters.OrderBy, fullMedia) ||
                FieldsAreInvalid(parameters.Fields, fullMedia));
        }

        private IEnumerable<ExpandoObject> GetShapedAuthors(PagedList<Author> authors,
            string fields, bool fullMedia)
        {
            dynamic mappedAuthors;

            if (fullMedia)
            {
                mappedAuthors = mapper.Map<IEnumerable<AuthorOutputDto>>(authors);
                GetPostsAndCommentsCountForAuthors(mappedAuthors.ToArray());
            }
            else
                mappedAuthors = mapper.Map<IEnumerable<AuthorShortOutputDto>>(authors);

            return properties.ShapeData(mappedAuthors, fields);
        }

        private dynamic GetLinkedAuthorsWithLinks(PagedList<Author> authors,
            IEnumerable<ExpandoObject> shapedAuthors, BasicResourceParameters parameters)
        {
            var authorsIds = GetIds(authors);

            return new
            {
                value = GetLinkedAuthors(shapedAuthors, authorsIds),
                links = CreateLinksForAuthors(authors, parameters)
            };
        }

        private List<Guid> GetIds(PagedList<Author> authors) =>
            authors.Select(a => a.Id).ToList();

        private IEnumerable<IDictionary<string, object>> GetLinkedAuthors
            (IEnumerable<ExpandoObject> shapedAuthors, List<Guid> authorsIds)
        {
            var counter = 0;

            return shapedAuthors.Select(TransformIntoDictionary);

            IDictionary<string, object> TransformIntoDictionary(ExpandoObject author)
            {
                var authorLink = CreateLinksForAuthor(authorsIds[counter], null);
                var authorAsDictionary = author as IDictionary<string, object>;
                authorAsDictionary.Add("links", authorLink);

                counter++;
                return authorAsDictionary;
            }
        }

        private IEnumerable<LinkDto> CreateLinksForAuthors(PagedList<Author> authors, IResourceParameters parameters)
        {
            var links = new List<LinkDto>();
            var uriResource = new UriResource(Url, "GetAuthors");
            var resourceUri = pagination.CreateResourceUri(parameters, uriResource, ResourceUriType.Current);

            links.Add(new LinkDto(resourceUri, "self", "GET"));

            var pagesLinks = pagination.CreatePagesLinks(authors, parameters, uriResource);

            if (pagesLinks.HasPrevious)
                links.Add(new LinkDto(pagesLinks.Previous, "previousPage", "GET"));

            if (pagesLinks.HasNext)
                links.Add(new LinkDto(pagesLinks.Next, "nextPage", "GET"));

            return links;
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public ActionResult<AuthorOutputDto> GetAuthor
            ([FromHeader(Name = "Accept")] string mediaType, Guid id, string fields)
        {
            var parsedMediaType = GetMediaType(mediaType);
            var fullMedia = FullMedia(parsedMediaType);

            if (parsedMediaType is null || FieldsAreInvalid(fields, fullMedia))
                return BadRequest();

            var authorFromRepo = blogRepository.GetAuthor(id);

            if (authorFromRepo is null)
                return NotFound();

            var shapedAuthor = GetShapedAuthor(authorFromRepo, fields, fullMedia);

            if (IncludeLinks(parsedMediaType))
            {
                var links = CreateLinksForAuthor(authorFromRepo.Id, fields);
                var authorAsDictionary = shapedAuthor as IDictionary<string, object>;
                authorAsDictionary.Add("links", links);

                return Ok(authorAsDictionary);
            }

            return Ok(shapedAuthor);
        }

        private MediaTypeHeaderValue GetMediaType(string mediaType)
        {
            MediaTypeHeaderValue.TryParse(mediaType,
                out MediaTypeHeaderValue parsedMediaType);

            return parsedMediaType;
        }

        private bool FullMedia(MediaTypeHeaderValue headerValue) =>
            headerValue.MediaType == Constants.AuthorFullMediaType ||
                headerValue.MediaType == Constants.AuthorFullHateoasMediaType;

        private ExpandoObject GetShapedAuthor
            (Author author, string fields, bool fullMedia)
        {
            dynamic mappedAuthor;

            if (fullMedia)
            {
                mappedAuthor = mapper.Map<AuthorOutputDto>(author);
                GetPostsAndCommentsCountForAuthors(mappedAuthor);
            }
            else
                mappedAuthor = mapper.Map<AuthorShortOutputDto>(author);

            return properties.ShapeSingleData(mappedAuthor, fields);
        }

        private void GetPostsAndCommentsCountForAuthors(params AuthorOutputDto[] authors)
        {
            foreach (var author in authors)
            {
                author.PostsNumber
                    = blogRepository.GetPostsNumberForAuthor(author.Id);
                author.CommentsNumber
                    = blogRepository.GetCommentsNumberForAuthor(author.Id);
            }
        }

        private bool IncludeLinks(MediaTypeHeaderValue mediaType) =>
            mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
                StringComparison.InvariantCultureIgnoreCase);

        private IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(new LinkDto(Url.Link("GetAuthor",
                    new { id = authorId }), "self", "GET"));
            else
                links.Add(new LinkDto(Url.Link("GetAuthor",
                new { id = authorId, fields }), "self", "GET"));

            return links;
        }

        private bool FieldsAreInvalid(string fields, bool fullMedia = true)
        {
            if (fullMedia)
                return validator.FieldsAreInvalid<AuthorOutputDto>(fields);
            else
                return validator.FieldsAreInvalid<AuthorShortOutputDto>(fields);
        }

        private bool OrderIsInvalid(string order, bool fullMedia = true)
        {
            if (fullMedia)
                return validator.OrderIsInvalid<Author, AuthorOutputDto>(order);
            else
                return validator.OrderIsInvalid<Author, AuthorShortOutputDto>(order);
        }
    }
}