using anBlogg.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anBlogg.Infrastructure.Persistence.Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .OwnsOne(p => p.Tags);

            builder
                .OwnsOne(p => p.Score);

            builder
                .HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Comments)
                .WithOne(p => p.Post)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}