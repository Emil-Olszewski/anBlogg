using FluentValidation;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase
    {
    }
}