using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Infrastructure.FluentValidation;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace anBlogg.WebApi.Controllers.Common
{
    public abstract class CustomControllerBase : ControllerBase
    {
        protected readonly IValidator validator;
        protected readonly IMapper mapper;
        protected readonly IBlogRepository blogRepository;
        protected readonly IPropertyMappingService mappingService;
        protected readonly IProperties properties;
        protected readonly IPagination pagination;

        public CustomControllerBase(IMapper mapper, IBlogRepository blogRepository,
            IValidator validator, IProperties properties, IPagination pagination)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
            this.validator = validator;
            this.properties = properties;
            this.pagination = pagination;
        }

        protected void AddHeader<T>(PagedList<T> source)
        {
            var header = pagination.CreateHeader(source);
            Response.Headers.Add(header.Name, header.Value);
        }
    }
}