using anBlogg.Application.Services;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Helpers;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using anBlogg.WebApi.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using anBlogg.Application.Services.Helpers;
using anBlogg.Domain.Entities;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : PostsControllerBase
    {
        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository)
            : base(mapper, blogRepository) { }

        [HttpGet(Name = "GetPosts")]
        public ActionResult<IEnumerable<PostOutputDto>> GetPosts
            ([FromQuery] PostResourceParameters parameters)
        {
            if (CantValidate(parameters))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPosts(parameters);

            var header = CreatePaginationHeader(postsFromRepo, parameters);
            Response.Headers.Add(header.name, header.value);

            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);

            return Ok(mappedPosts);
        }

        private bool CantValidate(PostResourceParameters input)
        {
            return Validator.CantValidate
                (new PostResourceParametersValidator(), input, ModelState);
        }

        private dynamic CreatePaginationHeader
            (PagedList<Post> posts, PostResourceParameters parameters)
        {
            var previousPageLink = posts.HasPrevious ?
                CreatePostsResourceUri(parameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = posts.HasNext ?
               CreatePostsResourceUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = posts.TotalCount,
                pageSize = posts.PageSize,
                currentPage = posts.CurrentPage,
                totalPages = posts.TotalPages,
                previousPageLink,
                nextPageLink
            };

            var serializedMetadata = JsonSerializer.Serialize(paginationMetadata);
            return new { name = "X-Pagination", value = serializedMetadata };
        }

        private string CreatePostsResourceUri
            (PostResourceParameters parameters, ResourceUriType type)
        {
            var valueToAdd = GetValueToAddDependingOn(type);
            var resource = new
            {
                pageNumber = parameters.PageNumber + valueToAdd,
                pageSize = parameters.PageSize,
                tags = parameters.Tags
            };

            return Url.Link("GetPosts", resource);
        }

        private int GetValueToAddDependingOn(ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => -1,
                ResourceUriType.NextPage => 1,
                _ => 0
            };
        } 
    }
}