using FluentValidation;
using FluentValidation.Validators;
using System;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    public interface IValidatorBase
    {
    }

    public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase
    {
        protected string? guidFieldName;
        protected void IsAValidGuid(string id, CustomContext context)
        {
            if (!Guid.TryParse(id, out _))
                context.AddFailure(guidFieldName, "This is not a valid guid");
        }
    }
}