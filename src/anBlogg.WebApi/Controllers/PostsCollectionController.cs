using anBlogg.Application.Services;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : PostsControllerBase
    {
        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository,
           IValidator validator, IProperties properties, IPagination pagination)
           : base(mapper, blogRepository, validator, properties, pagination)
        {
        }

        [HttpGet(Name = "GetAllPosts")]
        public ActionResult<IEnumerable<IPostOutputDto>> GetPosts(
            [FromHeader(Name = "Content-Type")] string mediaType,
            [FromQuery] PostResourceParameters parameters)
        {
            var user = this.User;

            if (CantValidate(parameters))
                return BadRequest();

            if (validator.DontMatchRules(parameters, ModelState))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPosts(parameters);
            InsertAuthorsInto(postsFromRepo.ToArray());

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            InsertCommentsNumberInto(mappedPosts.ToArray());

            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);

            AddPaginationHeader(postsFromRepo);


            if (IncludeLinks(mediaType))
            {
                var linkedPosts = GetCollectionWithLinks
                    (postsFromRepo, shapedPosts, parameters);
                return Ok(linkedPosts);
            }

            return Ok(shapedPosts);
        }

        protected bool CantValidate(IPostResourceParameters parameters)
        {
            return (validator.OrderIsInvalid<Post, IPostOutputDto>(parameters.OrderBy) ||
                validator.FieldsAreInvalid<IPostOutputDto>(parameters.Fields));
        }
    }
}