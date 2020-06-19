using System;
using System.Collections.Generic;

namespace anBlogg.Application.Services.Models
{
    public interface IPostOutputDto
    {
        IAuthorShortOutputDto Author { get; set; }
        string Contents { get; set; }
        DateTime Created { get; set; }
        Guid Id { get; set; }
        DateTime Modified { get; set; }
        int Score { get; set; }
        string[] Tags { get; set; }
        string Title { get; set; }
        int CommentsNumber { get; set; }
    }
}