using anBlogg.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace anBlogg.Application.Services
{
    public interface IBlogContext
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }

        int SaveChanges();
    }
}