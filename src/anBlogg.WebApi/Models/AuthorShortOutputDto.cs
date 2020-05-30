using anBlogg.Application.Services.Models;
using System;

namespace anBlogg.WebApi.Models
{
    public class AuthorShortOutputDto : IAuthorShortOutputDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public int Score { get; set; }
    }
}