using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using System;
using System.Linq;

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

        public ActionResult<IEnumerable<AuthorOutputDto>> GetAllAuthors() 
        {
            var authorsFromRepo = blogRepository.GetAllAuthors().ToList();
            var authorsToReturn = mapper.Map<IEnumerable<AuthorOutputDto>>(authorsFromRepo);
            GetPostAndCommentsCountForAuthors(authorsToReturn.ToArray());
            return Ok(authorsToReturn);
        }

        [HttpGet("{id}")]
        public ActionResult<AuthorOutputDto> GetAuthor(Guid id)
        {
            var authorFromRepo = blogRepository.GetAuthor(id);
            var authorToReturn = mapper.Map<AuthorOutputDto>(authorFromRepo);
            GetPostAndCommentsCountForAuthors(authorToReturn);
            return Ok(authorToReturn);
        }

        private void GetPostAndCommentsCountForAuthors(params AuthorOutputDto[] authors)
        {
            foreach (var author in authors)
            {
                author.PostsNumber
                    = blogRepository.GetPostsNumberForAuthor(author.Id);
                author.CommentsNumber
                    = blogRepository.GetCommentsNumberForAuthor(author.Id);
            }
        }
    }
}
