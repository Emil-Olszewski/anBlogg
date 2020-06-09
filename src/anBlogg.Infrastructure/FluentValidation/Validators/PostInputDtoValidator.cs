using anBlogg.Infrastructure.FluentValidation.Models;
using anBlogg.Infrastructure.FluentValidation.Validators;
using FluentValidation;

namespace anBlogg.WebApi.Validators
{
    public class PostInputDtoValidator : ValidatorBase<IPostInputDto>
    {
        public PostInputDtoValidator()
        {
            RuleFor(p => p.Title).NotEmpty();
            RuleFor(p => p.Contents).NotEmpty();
            RuleFor(p => p.Tags).NotEmpty();
        }
    }
}