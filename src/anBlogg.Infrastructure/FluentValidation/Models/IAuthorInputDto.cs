using System;

namespace anBlogg.Infrastructure.FluentValidation.Models
{
    public interface IAuthorInputDto
    {
        string DisplayName { get; set; }
        string Id { get; set; }
    }
}