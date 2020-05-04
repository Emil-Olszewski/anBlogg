using anBlogg.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anBlogg.Infrastructure.Persistence.Configurations
{
    class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .OwnsOne(a=> a.Score);

            builder
                .HasMany(a => a.Comments)
                .WithOne(c => c.Author)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(a => a.Posts)
                .WithOne(p => p.Author)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
