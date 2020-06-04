using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : PostsControllerBase
    {
        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository,
            IPropertyMappingService mappingService, IProperties properties, IPagination pagination)
            : base(mapper, blogRepository, mappingService, properties, pagination)
        { }

        [HttpGet(Name = "GetPosts")]
        public ActionResult<IEnumerable<IPostOutputDto>> GetPosts
            ([FromQuery] PostResourceParameters parameters)
        {
            if (CantValidate(parameters))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPosts(parameters);

            var header = pagination.CreateHeader(postsFromRepo, parameters, new UriResource(Url));
            Response.Headers.Add(header.Name, header.Value);

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);
            return Ok(shapedPosts);
        }
    }
}