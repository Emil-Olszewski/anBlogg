using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBlogRepository blogRepository;

        public CommentsController(IMapper mapper, IBlogRepository blogRepository)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CommentOutputDto>> GetCommentsForAuthor(Guid authorId)
        {
            if (blogRepository.AuthorNotExist(authorId))
                return NotFound();

            var commentsFromRepo = blogRepository.GetCommentsForAuthor(authorId);
            var mappedComments = mapper.Map<IEnumerable<CommentOutputDto>>(commentsFromRepo);
            return Ok(mappedComments);
        }
    }
}