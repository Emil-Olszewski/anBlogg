using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Cors;

namespace anBlogg.WebApi.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBlogRepository blogRepository;

        public AuthorsController(IMapper mapper, IBlogRepository blogRepository)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
        }

        //public ActionResult<IEnumerable<AuthorOutputDto>> GetAllAuthors()
        //{
        //    var authorsFromRepo = blogRepository.GetAllAuthors();
        //    var authorsToReturn = mapper.Map<IEnumerable<AuthorOutputDto>>(authorsFromRepo);

        //    foreach (var author in authorsToReturn)
        //    {
        //        author.NumberOfPosts 
        //            = blogRepository.GetNumberOfPostsForAuthor(author.Id);
        //        author.NumberOfComments 
        //            = blogRepository.GetNumberOfCommentsForAuthor(author.Id);
        //    }

        //    return Ok(authorsToReturn);
        //}

        [HttpGet()]
        public ActionResult<IEnumerable<AuthorOutputDto>> GetAuthors([FromQuery] Guid[] authorsIds)
        {
            var authorsFromRepo = blogRepository.GetAuthors(authorsIds);
            var authorsToReturn = mapper.Map<IEnumerable<AuthorOutputDto>>(authorsFromRepo);
            return Ok(authorsToReturn);
        }
    }
}
