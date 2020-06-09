using anBlogg.Application.Services.Models;
using anBlogg.Domain;
using FluentValidation;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    public class PostResourceParametersValidator : ValidatorBase<IPostResourceParameters>
    {
        public PostResourceParametersValidator()
        {
            RuleFor(p => p.PageSize).LessThanOrEqualTo(Constants.MaxPageSize);
            RuleFor(p => p.PageSize).GreaterThan(0);
            RuleFor(p => p.PageNumber).GreaterThan(0);
        }
    }
}