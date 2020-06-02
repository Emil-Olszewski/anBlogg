using anBlogg.Application.Services;
using anBlogg.WebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace anBlogg.WebApi.Controllers.Common
{
    public class PostsControllerBase : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly IBlogRepository blogRepository;

        public PostsControllerBase(IMapper mapper, IBlogRepository blogRepository)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
        }
    }
}