using anBlogg.Application.Services;
using anBlogg.Infrastructure.FluentValidation.Validators;
using anBlogg.WebApi.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.Infrastructure.FluentValidation
{
    public class Validator : IValidator
    {
        private readonly List<IValidatorBase> validators;
        private readonly IProperties properties;
        private readonly IPropertyMappingService mappingService;

        public Validator(IProperties properties, IPropertyMappingService mappingService)
        {
            validators = new List<IValidatorBase>
            {
                new PostResourceParametersValidator(),
                new PostInputDtoValidator()
            };

            this.properties = properties;
            this.mappingService = mappingService;
        }

        public bool DontMatchRules<T>(T input, ModelStateDictionary? modelState = null)
        {
            var validator = validators.OfType<ValidatorBase<T>>().FirstOrDefault();
            var validationResult = validator?.Validate(input);

            if (validator is null || validationResult.IsValid)
                return false;

            if (modelState != null)
                validationResult.AddToModelState(modelState, null);

            return true;
        }

        public bool FieldsAreInvalid<T>(string fields)
        {
            return !string.IsNullOrWhiteSpace(fields) &&
                properties.NotExistsIn<T>(fields);
        }

        public bool OrderIsInvalid<TSource, TDestination>(string orderBy)
        {
            return !string.IsNullOrWhiteSpace(orderBy) && mappingService
                .MappingNotDefinedFor<TDestination, TSource>(orderBy);
        }
    }
}