using anBlogg.Application.Services;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using anBlogg.WebApi.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : PostsControllerBase
    {
        private readonly IPropertyMappingService mappingService;
        private readonly IProperties properties;
        private readonly IPagination pagination;

        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository,
            IPropertyMappingService mappingService, IProperties properties, IPagination pagination)
            : base(mapper, blogRepository)
        {
            this.mappingService = mappingService;
            this.properties = properties;
            this.pagination = pagination;
        }

        [HttpGet(Name = "GetPosts")]
        public ActionResult<IEnumerable<IPostOutputDto>> GetPosts
            ([FromQuery] PostResourceParameters parameters)
        {
            if (CantValidate(parameters))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPosts(parameters);

            var header = pagination.CreateHeader(postsFromRepo, parameters, Url);
            Response.Headers.Add(header.Name, header.Value);

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            var shapedPosts = properties.ShapeData(mappedPosts, parameters.Fields);
            return Ok(shapedPosts);
        }

        private bool CantValidate(PostResourceParameters input)
        {
            var areParametersInvalid = AreWrongParametersTyped(input);
            var areFieldsInvalid = AreWrongFieldsTyped(input.Fields);
            var isOrderByInvalid = AreWrongOrderByTyped(input.OrderBy);

            if (areParametersInvalid || areFieldsInvalid || isOrderByInvalid)
                return true;

            return false;
        }

        private bool AreWrongParametersTyped(PostResourceParameters parameters)
        {
            return Validator.CantValidate
                (new PostResourceParametersValidator(), parameters, ModelState);
        }

        private bool AreWrongFieldsTyped(string fields)
        {
            return !string.IsNullOrWhiteSpace(fields) &&
                properties.NotExistsIn<IPostOutputDto>(fields);
        }

        private bool AreWrongOrderByTyped(string orderBy)
        {
            return !string.IsNullOrWhiteSpace(orderBy) && mappingService
                .MappingNotDefinedFor<IPostOutputDto, Post>(orderBy);
        }
    }
}