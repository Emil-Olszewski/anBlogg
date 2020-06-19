using System;

namespace anBlogg.Infrastructure.FluentValidation.Models
{
    public interface ICommentInputDto
    {
        string AuthorId { get; set; }

        string Contents { get; set; }
    }
}