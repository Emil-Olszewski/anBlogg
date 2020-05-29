using anBlogg.WebApi.ResourceParameters;
using FluentValidation;

namespace anBlogg.WebApi.Validators
{
    public class PostResourceParametersValidator : AbstractValidator<PostResourceParameters>
    {
        public PostResourceParametersValidator()
        {
            RuleFor(p => p.PostsDisplayed).LessThanOrEqualTo(Constants.MaxPostsOnPage);
            RuleFor(p => p.PostsDisplayed).GreaterThan(0);
            RuleFor(p => p.PageNumber).GreaterThan(0);
        }
    }
}