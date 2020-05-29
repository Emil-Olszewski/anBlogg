using anBlogg.WebApi.Models;
using FluentValidation;

namespace anBlogg.WebApi.Validators
{
    public class PostInputDtoValidator : AbstractValidator<PostInputDto>
    {
        public PostInputDtoValidator()
        {
            RuleFor(p => p.Title).NotEmpty();
            RuleFor(p => p.Contents).NotEmpty();
            RuleFor(p => p.Tags).NotEmpty();
        }
    }
}