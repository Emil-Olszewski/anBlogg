using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.Infrastructure.FluentValidation.Models;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace anBlogg.WebApi.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/authors/{authorId}/posts")]
    public class PostsController : CustomControllerBase
    {
        private Guid currentAuthorId;
        private Guid currentPostId;

        public PostsController(IMapper mapper, IBlogRepository blogRepository,
           IValidator validator, IProperties properties, IPagination pagination)
           : base(mapper, blogRepository, validator, properties, pagination)
        {
        }

        [HttpOptions]
        public IActionResult GetPostOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,PATCH,DELETE,OPTIONS");
            return Ok();
        }

        [HttpGet(Name = "GetPostsForAuthor")]
        [HttpHead]
        public ActionResult<IEnumerable<PostOutputDto>> GetPostsForAuthor
            (Guid authorId, [FromQuery] PostResourceParameters parameters)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            if (CantValidate(parameters))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPostsForAuthor(authorId, parameters);

            var header = pagination.CreateHeader(postsFromRepo);
            Response.Headers.Add(header.Name, header.Value);

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);

            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);

            var links = CreateLinksForPosts(postsFromRepo, parameters);

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

        [HttpGet("{postId}", Name = "GetPost")]
        public ActionResult<PostOutputDto> GetPostForAuthor
            (Guid authorId, Guid postId, PostResourceParameters parameters)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            if (CantValidate(parameters))
                return ValidationProblem();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);
            if (postFromRepo is null)
                return NotFound();

            var mappedPost = mapper.Map<PostOutputDto>(postFromRepo);
            var shapedPost = properties.ShapeSingleData(mappedPost, parameters.Fields);

            var links = CreateLinksForPost(authorId, postId, parameters.Fields);
            var linkedsResourceToReturn = shapedPost as IDictionary<string, object>;

            linkedsResourceToReturn.Add("links", links);

            return Ok(linkedsResourceToReturn);
        }

        [HttpPost(Name = "CreatePost")]
        public IActionResult AddPostForAuthor(Guid authorId, PostInputDto postToAdd)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            RememberIds(authorId);
            return ValidateThenAddOrUpdate(null, postToAdd);
        }

        [HttpPut("{postId}")]
        public IActionResult UpdatePostForAuthor(Guid authorId, Guid postId, PostInputDto postToUpdate)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);

            RememberIds(authorId, postId);
            return ValidateThenAddOrUpdate(postFromRepo, postToUpdate);
        }

        [HttpPatch("{postId}")]
        public IActionResult PartiallyUpdatePostForAuthor
            (Guid authorId, Guid postId, JsonPatchDocument<PostInputDto> patchDocument)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);
            var postDto = mapper.Map<PostInputDto>(postFromRepo) ?? new PostInputDto();
            patchDocument.ApplyTo(postDto, ModelState);

            RememberIds(authorId, postId);
            return ValidateThenAddOrUpdate(postFromRepo, postDto);
        }

        private void RememberIds(Guid authorId, Guid? postId = null)
        {
            currentAuthorId = authorId;

            if (postId is null)
                currentPostId = Guid.Empty;
            else
                currentPostId = (Guid)postId;
        }

        private IActionResult ValidateThenAddOrUpdate(Post postFromRepo, IPostInputDto postDto)
        {
            if (validator.DontMatchRules(postDto))
                return ValidationProblem(ModelState);

            if (postFromRepo is null)
                return AddPost(postDto);
            else
                return UpdatePost(postDto, postFromRepo);
        }

        private IActionResult AddPost(IPostInputDto postDto)
        {
            var postToAdd = mapper.Map<Post>(postDto);

            if (currentPostId != Guid.Empty)
                postToAdd.Id = currentPostId;

            blogRepository.AddPostForAuthor(currentAuthorId, postToAdd);
            blogRepository.SaveChanges();

            var mappedPost = mapper.Map<PostOutputDto>(postToAdd);
            var shapedPost = properties.ShapeSingleData(mappedPost, null);

            var links = CreateLinksForPost(currentAuthorId, mappedPost.Id, null);
            var linkedResourceToReturn = shapedPost as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetPost",
                new { authorId = currentAuthorId, postId = mappedPost.Id }, linkedResourceToReturn);
        }

        private IActionResult UpdatePost(IPostInputDto postDto, Post postFromRepo)
        {
            mapper.Map(postDto, postFromRepo);

            blogRepository.UpdatePostForAuthor(currentAuthorId, currentPostId);
            blogRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{postId}", Name = "DeletePost")]
        public ActionResult DeletePostForAuthor(Guid authorId, Guid postId)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var post = blogRepository.GetPostForAuthor(authorId, postId);
            if (post is null)
                return NotFound();

            blogRepository.DeletePost(post);
            blogRepository.SaveChanges();

            return NoContent();
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
            var uriResource = new UriResource(Url, "GetPostsForAuthor");
            var resourceUri = pagination.CreateResourceUri(parameters, uriResource, ResourceUriType.Current);

            links.Add(new LinkDto(resourceUri, "self", "GET"));

            var pagesLinks = pagination.CreatePagesLinks(posts, parameters, uriResource);

            if (pagesLinks.HasPrevious)
                links.Add(new LinkDto(pagesLinks.Previous, "previousPage", "GET"));

            if (pagesLinks.HasNext)
                links.Add(new LinkDto(pagesLinks.Next, "nextPage", "GET"));

            return links;
        }

        protected bool CantValidate(IPostResourceParameters parameters)
        {
            return (validator.DontMatchRules(parameters) ||
                validator.OrderIsInvalid<Post, IPostOutputDto>(parameters.OrderBy) ||
                validator.FieldsAreInvalid<IPostOutputDto>(parameters.Fields));
        }
    }
}