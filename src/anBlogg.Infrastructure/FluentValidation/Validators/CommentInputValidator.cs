using anBlogg.Infrastructure.FluentValidation.Models;
using FluentValidation;
using FluentValidation.Validators;
using System;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    public class CommentInputValidator : ValidatorBase<ICommentInputDto>
    {
        public CommentInputValidator()
        {
            RuleFor(comment => comment.AuthorId).NotEmpty();
            RuleFor(comment => comment.AuthorId).Custom(IsAValidGuid) ;
            RuleFor(comment => comment.Contents).NotEmpty();
        }

        private void IsAValidGuid(string id, CustomContext context)
        {
            if (!Guid.TryParse(id, out _))
                context.AddFailure("authorId", "This is not a valid guid");
        }
    }
}
