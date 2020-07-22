using anBlogg.Application.Services;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.Infrastructure.FluentValidation.Models;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Helpers;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.WebApi.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/authors/{authorId}/posts")]
    public class PostsController : PostsControllerBase
    {
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

        [HttpGet(Name = "GetPosts")]
        [HttpHead]
        public ActionResult<IEnumerable<PostOutputDto>> GetPosts
            (Guid authorId, [FromQuery] PostResourceParameters parameters,
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (CantValidateParameters(parameters))
                return BadRequest();

            if (validator.DontMatchRules(parameters, ModelState))
                return ValidationProblem();

            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postsFromRepo = blogRepository.GetPostsForAuthor(authorId, parameters);
            InsertAuthorsInto(postsFromRepo.ToArray());

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            InsertCommentsNumberInto(mappedPosts.ToArray());

            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);

            AddPaginationHeader(postsFromRepo);

            if (IncludeLinks(mediaType))
            {
                var linkedPosts = GetCollectionWithLinks(postsFromRepo, shapedPosts, parameters);
                return Ok(linkedPosts);
            }

            return Ok(shapedPosts);
        }

        private bool CantValidateParameters(PostResourceParameters parameters)
        {
            return validator.FieldsAreInvalid<PostOutputDto>(parameters.Fields) ||
                validator.OrderIsInvalid<Post, IPostOutputDto>(parameters.OrderBy);
        }

        [HttpGet("{postId}", Name = "GetPost")]
        public ActionResult<PostOutputDto> GetPost
            (Guid authorId, Guid postId, string fields,
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (validator.FieldsAreInvalid<PostOutputDto>(fields))
                return BadRequest();

            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);
            if (postFromRepo is null)
                return NotFound();

            InsertAuthorsInto(postFromRepo);

            var mappedPost = mapper.Map<PostOutputDto>(postFromRepo);
            InsertCommentsNumberInto(mappedPost);

            var shapedPost = properties.ShapeSingleData(mappedPost, fields);

            if (IncludeLinks(mediaType))
            {
                var idsSet = new PostIdsSet(authorId, postId);
                var linkedResource = GetLinkedResource(shapedPost, idsSet, fields);
                return Ok(linkedResource);
            }

            return Ok(shapedPost);
        }

        [Authorize]
        [HttpPost(Name = "CreatePost")]
        public IActionResult AddPost(Guid authorId, PostInputDto newPost,
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (IsUserIdNotEqualTo(authorId))
                return Unauthorized();

            var idsSet = new PostIdsSet(authorId);
            return AddPost(idsSet, newPost, IncludeLinks(mediaType));
        }

        [Authorize]
        [HttpPut("{postId}", Name = "UpdatePost")]
        public IActionResult UpdatePost(Guid authorId, Guid postId, PostInputDto updatedPost,
             [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (IsUserIdNotEqualTo(authorId))
                return Unauthorized();

            var idsSet = new PostIdsSet(authorId, postId);
            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);

            if (postFromRepo is null)
                return AddPost(idsSet, updatedPost, IncludeLinks(mediaType));

            mapper.Map(updatedPost, postFromRepo);
            blogRepository.SaveChanges();

            if (IncludeLinks(mediaType))
            {
                var mappedPost = mapper.Map<PostOutputDto>(postFromRepo);
                return Ok(ShapeAndLinkSinglePost(mappedPost, idsSet));
            }

            return NoContent();
        }

        private IActionResult AddPost(PostIdsSet idsSet, PostInputDto newPost, bool includeLinks)
        {
            if (validator.DontMatchRules(newPost as IPostInputDto, ModelState))
                return ValidationProblem(ModelState);

            if (blogRepository.AuthorNotExist(idsSet.authorId))
                return NotFound();

            var postToAdd = mapper.Map<Post>(newPost);
            postToAdd.AuthorId = idsSet.authorId;
            InsertAuthorsInto(postToAdd);

            if (idsSet.postId != Guid.Empty)
                postToAdd.Id = idsSet.postId;

            blogRepository.AddPostForAuthor(idsSet.authorId, postToAdd);
            blogRepository.SaveChanges();

            var mappedPost = mapper.Map<PostOutputDto>(postToAdd);
            idsSet.postId = mappedPost.Id;

            dynamic toReturn = mappedPost;

            if (includeLinks)
                toReturn = ShapeAndLinkSinglePost(mappedPost, idsSet);

            return CreatedAtRoute("GetPost",
                new { idsSet.authorId, idsSet.postId }, toReturn);
        }

        [Authorize]
        [HttpPatch("{postId}", Name = "PatchPost")]
        public IActionResult PartiallyUpdatePostForAuthor
            (Guid authorId, Guid postId, JsonPatchDocument<PostInputDto> patchDocument,
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (IsUserIdNotEqualTo(authorId))
                return Unauthorized();

            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);
            if (postFromRepo is null)
                return NotFound();

            var postInputDto = mapper.Map<PostInputDto>(postFromRepo);
            patchDocument.ApplyTo(postInputDto, ModelState);

            if (validator.DontMatchRules(postInputDto as IPostInputDto, ModelState))
                return ValidationProblem(ModelState);

            mapper.Map(postInputDto, postFromRepo);
            blogRepository.SaveChanges();

            if (IncludeLinks(mediaType))
            {
                var idsSet = new PostIdsSet(authorId, postFromRepo.Id);
                var mappedPost = mapper.Map<PostOutputDto>(postFromRepo);
                return Ok(ShapeAndLinkSinglePost(mappedPost, idsSet));
            }

            return NoContent();
        }

        private IDictionary<string, object> ShapeAndLinkSinglePost
            (PostOutputDto postToReturn, PostIdsSet idsSet)
        {
            var shapedPost = properties.ShapeSingleData(postToReturn);
            return GetLinkedResource(shapedPost, idsSet);
        }

        [Authorize]
        [HttpDelete("{postId}", Name = "DeletePost")]
        public ActionResult DeletePostForAuthor(Guid authorId, Guid postId)
        {
            if (IsUserIdNotEqualTo(authorId))
                return Unauthorized();

            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);
            if (postFromRepo is null)
                return NotFound();

            blogRepository.DeletePost(postFromRepo);
            blogRepository.SaveChanges();

            return NoContent();
        }
    }
}