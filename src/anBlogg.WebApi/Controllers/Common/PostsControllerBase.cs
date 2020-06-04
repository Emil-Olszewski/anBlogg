using anBlogg.Application.Services;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using anBlogg.WebApi.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace anBlogg.WebApi.Controllers.Common
{
    public class PostsControllerBase : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly IBlogRepository blogRepository;
        protected readonly IPropertyMappingService mappingService;
        protected readonly IProperties properties;
        protected readonly IPagination pagination;

        public PostsControllerBase(IMapper mapper, IBlogRepository blogRepository, 
            IPropertyMappingService mappingService, IProperties properties, IPagination pagination)
        {
            this.mapper = mapper;
            this.blogRepository = blogRepository;
            this.mappingService = mappingService;
            this.properties = properties;
            this.pagination = pagination;
        }

        protected bool CantValidate(PostResourceParameters input)
        {
            var areParametersInvalid = AreWrongParametersTyped(input);
            var areFieldsInvalid = AreWrongFieldsTyped(input.Fields);
            var isOrderByInvalid = AreWrongOrderByTyped(input.OrderBy);

            if (areParametersInvalid || areFieldsInvalid || isOrderByInvalid)
                return true;

            return false;
        }

        protected bool AreWrongParametersTyped(PostResourceParameters parameters)
        {
            return Validator.CantValidate
                (new PostResourceParametersValidator(), parameters, ModelState);
        }

        protected bool AreWrongFieldsTyped(string fields)
        {
            return !string.IsNullOrWhiteSpace(fields) &&
                properties.NotExistsIn<IPostOutputDto>(fields);
        }

        protected bool AreWrongOrderByTyped(string orderBy)
        {
            return !string.IsNullOrWhiteSpace(orderBy) && mappingService
                .MappingNotDefinedFor<IPostOutputDto, Post>(orderBy);
        }
    }
}