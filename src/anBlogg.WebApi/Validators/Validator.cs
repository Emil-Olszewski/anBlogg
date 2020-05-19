using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace anBlogg.WebApi.Validators
{
    public static class Validator
    {
        public static bool CantValidate<T>
            (AbstractValidator<T> validator, T input, ModelStateDictionary ModelState)
        {
            var validationResult = validator.Validate(input);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return true;
            }

            return false;
        }
    }
}
