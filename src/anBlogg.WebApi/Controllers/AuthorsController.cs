using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
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

        public ActionResult<IEnumerable<AuthorOutputDto>> GetAuthors()
        {
            var authorsFromRepo = blogRepository.GetAllAuthors();
            var authorsToReturn = mapper.Map<IEnumerable<AuthorOutputDto>>(authorsFromRepo);

            foreach (var author in authorsToReturn)
            {
                author.NumberOfPosts 
                    = blogRepository.GetNumberOfPostsForAuthor(author.Id);
                author.NumberOfComments 
                    = blogRepository.GetNumberOfCommentsForAuthor(author.Id);
            }

            return Ok(authorsToReturn);
        }
    }
}
