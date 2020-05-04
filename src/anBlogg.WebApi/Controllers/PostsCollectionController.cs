using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBlogRepository blogRepository;

        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<PostOutputDto>> GetPosts([FromQuery] PostResourceParameters parameters)
        {
            var postsFromRepo = blogRepository.GetAllPosts(parameters);
            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            return Ok(mappedPosts);
        }
    }
}
