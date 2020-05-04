using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using anBlogg.WebApi.Validators;
using FluentValidation.AspNetCore;
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

        public PostsController(IMapper mapper, IBlogRepository blogRepository)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
        }

        [HttpGet()]
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
        public ActionResult<PostOutputDto> AddPostForAuthor(Guid authorId, PostInputDto postToAdd)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var validator = new PostInputDtoValidator();
            var validationResult = validator.Validate(postToAdd);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return BadRequest(ModelState);
            }

            var postEntity = mapper.Map<Post>(postToAdd);
            blogRepository.AddPostForAuthor(authorId, postEntity);
            blogRepository.SaveChanges();

            var postToReturn = mapper.Map<PostOutputDto>(postEntity);
            return CreatedAtRoute
                ("GetPostForAuthor", new { authorId, postId = postToReturn.Id}, postToReturn);
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
