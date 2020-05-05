using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using anBlogg.WebApi.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBlogRepository blogRepository;
        private Guid currentAuthorId;
        private Guid currentPostId;

        public PostsController(IMapper mapper, IBlogRepository blogRepository)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,PATCH,DELETE,OPTIONS");
            return Ok();
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<PostOutputDto>> GetPostsForAuthor
            (Guid authorId, [FromQuery] PostResourceParameters parameters)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postsFromRepo = blogRepository.GetAllPostsForAuthor(authorId, parameters);
            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            return Ok(mappedPosts);
        }

        [HttpGet("{postId}", Name = "GetPostForAuthor")]
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

            if (CantValidate(postToAdd))
                return BadRequest(ModelState);

            RememberIds(authorId);
            return ValidateThenAddPost(postToAdd);
        }

        [HttpPut("{postId}")]
        public IActionResult UpdatePostForAuthor(Guid authorId, Guid postId, PostInputDto postToUpdate)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var postFromRepo = blogRepository.GetPostForAuthor(authorId, postId);

            RememberIds(authorId, postId);
            return AddOrUpdate(postFromRepo, postToUpdate);
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
            return AddOrUpdate(postFromRepo, postDto);
        }

        private IActionResult AddOrUpdate(Post postFromRepo, PostInputDto postDto)
        {
            if (postFromRepo is null)
                return ValidateThenAddPost(postDto);
            else
                return ValidateThenUpdatePost(postDto, postFromRepo);
        }

        private void RememberIds(Guid authorId, Guid postId = new Guid())
        {
            currentAuthorId = authorId;
            currentPostId = postId;
        }

        private IActionResult ValidateThenAddPost(PostInputDto postDto)
        {
            if (CantValidate(postDto))
                return ValidationProblem(ModelState);

            var postToAdd = mapper.Map<Post>(postDto);
            if (currentPostId != Guid.Empty)
                postToAdd.Id = currentPostId;

            blogRepository.AddPostForAuthor(currentAuthorId, postToAdd);
            blogRepository.SaveChanges();

            var postToReturn = mapper.Map<PostOutputDto>(postToAdd);
            return CreatedAtRoute("GetPostForAuthor",
                new { authorId = currentAuthorId, postId = postToReturn.Id }, postToReturn);
        }

        private IActionResult ValidateThenUpdatePost(PostInputDto postDto, Post postFromRepo)
        {
            if (CantValidate(postDto))
                return ValidationProblem(ModelState);

            mapper.Map(postDto, postFromRepo);

            blogRepository.UpdatePostForAuthor(currentAuthorId, currentPostId);
            blogRepository.SaveChanges();
            
            return NoContent();
        }

        private bool CantValidate(PostInputDto input)
        {
            var validator = new PostInputDtoValidator();
            var validationResult = validator.Validate(input);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return true;
            }

            return false;
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
