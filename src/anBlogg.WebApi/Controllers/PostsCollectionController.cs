﻿using AutoMapper;
using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using anBlogg.WebApi.Validators;
using System.Linq;
using anBlogg.WebApi.Controllers.Common;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsCollectionController : PostsControllerBase
    {

        public PostsCollectionController(IMapper mapper, IBlogRepository blogRepository) 
            : base(mapper, blogRepository) { }

        [HttpGet()]
        public ActionResult<IEnumerable<PostOutputDto>> GetPosts
            ([FromQuery] PostResourceParameters parameters)
        {
            if (CantValidate(parameters))
                return ValidationProblem(ModelState);

            var postsFromRepo = blogRepository.GetPosts(parameters);
            var mappedPosts = mapper.Map<IEnumerable<PostOutputDto>>(postsFromRepo);
            GetNumberOfCommentsFor(mappedPosts.ToArray());

            return Ok(mappedPosts);
        }

        private bool CantValidate(PostResourceParameters input)
        {
            return Validator.CantValidate
                (new PostResourceParametersValidator(), input, ModelState);
        }
    }
}
