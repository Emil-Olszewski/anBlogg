using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.Infrastructure.FluentValidation.Models;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Helpers;
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
    [Route("api/authors")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AuthorsController : CustomControllerBase
    {
        public AuthorsController(IMapper mapper, IBlogRepository blogRepository,
            IValidator validator, IProperties properties, IPagination pagination)
            : base(mapper, blogRepository, validator, properties, pagination)
        {
        }

        [HttpOptions]
        public IActionResult GetAuthorOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }

        [HttpGet(Name = "GetAuthors")]
        public ActionResult<IEnumerable<AuthorOutputDto>> GetAllAuthors(
            [FromHeader(Name = "Content-Type")] string mediaType,
            [FromQuery] BasicResourceParameters parameters)
        {
            var parsedMediaType = GetMediaType(mediaType);
            var fullMedia = FullMedia(parsedMediaType);

            if (CantValidate(parameters, fullMedia) || parsedMediaType is null)
                return BadRequest();

            var authorsFromRepo = blogRepository.GetAllAuthors(parameters);

            var shapedAuthors = GetShapedAuthors
                (authorsFromRepo, parameters.Fields, fullMedia);

            AddPaginationHeader(authorsFromRepo);

            dynamic toReturn = shapedAuthors;

            if (IncludeLinks(parsedMediaType))
                toReturn = GetCollectionWithLinks
                    (authorsFromRepo, shapedAuthors, parameters);

            return Ok(toReturn);
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

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public ActionResult<AuthorOutputDto> GetAuthor
            ([FromHeader(Name = "Content-Type")] string mediaType, Guid authorId, string fields)
        {
            var parsedMediaType = GetMediaType(mediaType);
            var fullMedia = FullMedia(parsedMediaType);

            if (parsedMediaType is null || FieldsAreInvalid(fields, fullMedia))
                return BadRequest();

            var authorFromRepo = blogRepository.GetAuthor(authorId);

            if (authorFromRepo is null)
                return NotFound();

            var shapedAuthor = GetShapedAuthor(authorFromRepo, fields, fullMedia);

            if (IncludeLinks(parsedMediaType))
            {
                var links = CreateLinksForSingleResource(new AuthorIdsSet(authorId), fields);
                var authorAsDictionary = shapedAuthor as IDictionary<string, object>;
                authorAsDictionary.Add("links", links);

                return Ok(authorAsDictionary);
            }

            return Ok(shapedAuthor);
        }

        private bool FullMedia(MediaTypeHeaderValue headerValue)
        {
            if (headerValue is null)
                return true;

            return headerValue.MediaType == Constants.AuthorFullMediaType ||
                headerValue.MediaType == Constants.AuthorFullHateoasMediaType;
        }

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

        protected override IEnumerable<LinkDto> CreateLinksForSingleResource(IIdsSet rawIds, string fields)
        {
            var ids = rawIds as AuthorIdsSet;
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(new LinkDto(Url.Link("GetAuthor",
                    new { ids.authorId }), "self", "GET"));
            else
                links.Add(new LinkDto(Url.Link("GetAuthor",
                    new { ids.authorId, fields }), "self", "GET"));

            return links;
        }

        protected override IEnumerable<IIdsSet> GetIds<T>(PagedList<T> resources)
        {
            var authors = resources as PagedList<Author>;
            return authors.Select(author => new AuthorIdsSet(author.Id));
        }

        [HttpPost(Name = "CreateAuthor")]
        public IActionResult CreateAuthor([FromBody]AuthorInputDto newAuthor, 
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (validator.DontMatchRules(newAuthor as IAuthorInputDto, ModelState))
                return BadRequest(ModelState);
            
            var authorToAdd = mapper.Map<Author>(newAuthor);

            var authorFromRepo = blogRepository.GetAuthor(authorToAdd.Id);
            if (authorFromRepo != null)
                return BadRequest();

            blogRepository.AddAuthor(authorToAdd);
            blogRepository.SaveChanges();

            var mappedAuthor = mapper.Map<AuthorOutputDto>(authorToAdd);
            dynamic toReturn = mappedAuthor;

            if (IncludeLinks(mediaType))
            {
                var shapedAuthor = properties.ShapeSingleData(mappedAuthor);
                var idsSet = new AuthorIdsSet(authorToAdd.Id);
                toReturn = GetLinkedResource(shapedAuthor, idsSet);
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorToAdd.Id }, toReturn);
        }
    }
}