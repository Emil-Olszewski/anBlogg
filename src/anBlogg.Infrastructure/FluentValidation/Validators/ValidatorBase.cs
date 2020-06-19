using FluentValidation;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    public interface IValidatorBase
    {
    }

    public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase
    {
    }
}