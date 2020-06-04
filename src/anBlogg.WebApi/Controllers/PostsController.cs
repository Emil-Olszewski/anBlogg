﻿using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using anBlogg.WebApi.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/authors/{authorId}/posts")]
    public class PostsController : PostsControllerBase
    {
        private Guid currentAuthorId;
        private Guid currentPostId;

        public PostsController(IMapper mapper, IBlogRepository blogRepository,
            IPropertyMappingService mappingService, IProperties properties, IPagination pagination)
            : base(mapper, blogRepository, mappingService, properties, pagination)
        { }

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

            var header = pagination.CreateHeader
                (postsFromRepo, parameters, new UriResource(Url, "GetPostsForAuthor"));
            Response.Headers.Add(header.Name, header.Value);

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);
            return Ok(shapedPosts);
        }

        [HttpGet("{postId}", Name = "GetPost")]
        public ActionResult<PostOutputDto> GetPostForAuthor(Guid authorId, Guid postId)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);
            if (postFromRepo is null)
                return NotFound();

            var mappedPost = mapper.Map<PostOutputDto>(postFromRepo);
            return Ok(mappedPost);
        }


        [HttpPost()]
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

        private void RememberIds(Guid authorId, Guid postId = new Guid())
        {
            currentAuthorId = authorId;
            currentPostId = postId;
        }

        private IActionResult ValidateThenAddOrUpdate(Post postFromRepo, PostInputDto postDto)
        {
            if (CantValidate(postDto))
                return ValidationProblem(ModelState);

            if (postFromRepo is null)
                return AddPost(postDto);
            else
                return UpdatePost(postDto, postFromRepo);
        }

        private IActionResult AddPost(PostInputDto postDto)
        {
            var postToAdd = mapper.Map<Post>(postDto);
            if (currentPostId != Guid.Empty)
                postToAdd.Id = currentPostId;

            blogRepository.AddPostForAuthor(currentAuthorId, postToAdd);
            blogRepository.SaveChanges();

            var postToReturn = mapper.Map<PostOutputDto>(postToAdd);
            return CreatedAtRoute("GetPost",
                new { authorId = currentAuthorId, postId = postToReturn.Id }, postToReturn);
        }

        private IActionResult UpdatePost(PostInputDto postDto, Post postFromRepo)
        {
            mapper.Map(postDto, postFromRepo);

            blogRepository.UpdatePostForAuthor(currentAuthorId, currentPostId);
            blogRepository.SaveChanges();

            return NoContent();
        }

        private bool CantValidate(PostInputDto input)
        {
            return Validator.CantValidate
                (new PostInputDtoValidator(), input, ModelState);
        }

        [HttpDelete("{postId}")]
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
    }
}