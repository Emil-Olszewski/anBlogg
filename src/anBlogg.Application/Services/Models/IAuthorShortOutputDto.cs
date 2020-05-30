using System;

namespace anBlogg.Application.Services.Models
{
    public interface IAuthorShortOutputDto
    {
        string DisplayName { get; set; }
        Guid Id { get; set; }
        int Score { get; set; }
    }
}