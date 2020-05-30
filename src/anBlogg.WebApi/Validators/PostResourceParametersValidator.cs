using anBlogg.WebApi.ResourceParameters;
using FluentValidation;

namespace anBlogg.WebApi.Validators
{
    public class PostResourceParametersValidator : AbstractValidator<PostResourceParameters>
    {
        public PostResourceParametersValidator()
        {
            RuleFor(p => p.PageSize).LessThanOrEqualTo(Constants.MaxPageSize);
            RuleFor(p => p.PageSize).GreaterThan(0);
            RuleFor(p => p.PageNumber).GreaterThan(0);
        }
    }
}