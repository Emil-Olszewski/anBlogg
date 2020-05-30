using System;

namespace anBlogg.Application.Services.Models
{
    public interface ICommentOutputDto
    {
        IAuthorShortOutputDto Author { get; set; }
        string Contents { get; set; }
        DateTime Created { get; set; }
        Guid Id { get; set; }
        int Score { get; set; }
    }
}