using anBlogg.Infrastructure.FluentValidation.Models;
using FluentValidation;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    public class CommentInputDtoValidator : ValidatorBase<ICommentInputDto>
    {
        public CommentInputDtoValidator()
        {
            RuleFor(comment => comment.Contents).NotEmpty();
        }
    }
}
