using anBlogg.Infrastructure.FluentValidation.Models;
using FluentValidation;

namespace anBlogg.Infrastructure.FluentValidation.Validators
{
    class AuthorInputDtoValidator : ValidatorBase<IAuthorInputDto>
    {
        public AuthorInputDtoValidator()
        {
            guidFieldName = "Id";
            RuleFor(author => author.Id).NotEmpty();
            RuleFor(author => author.Id).Custom(IsAValidGuid);
            RuleFor(author => author.DisplayName).NotEmpty();
        }
    }
}
