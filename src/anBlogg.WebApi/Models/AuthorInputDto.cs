using anBlogg.Infrastructure.FluentValidation.Models;
using System;

namespace anBlogg.WebApi.Models
{
    public class AuthorInputDto : IAuthorInputDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }
}
