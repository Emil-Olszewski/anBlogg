using anBlogg.Domain.Common;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace anBlogg.Infrastructure.Persistence
{
    public class BlogContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            modelBuilder.Entity<Author>().HasData(BlogContextSeeds.Authors);
            modelBuilder.Entity<Post>().HasData(BlogContextSeeds.Posts);
            modelBuilder.Entity<Post>().OwnsOne(p => p.Tags).HasData(BlogContextSeeds.Tags);

            modelBuilder.Entity<Comment>().HasData(BlogContextSeeds.Comments);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.Modified = DateTime.Now;
                        break;
                }
            }

            return base.SaveChanges();
        }
    }
}
