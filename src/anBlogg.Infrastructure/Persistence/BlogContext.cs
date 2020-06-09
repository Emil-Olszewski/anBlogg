using anBlogg.Application.Services;
using anBlogg.Domain.Common;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;

namespace anBlogg.Infrastructure.Persistence
{
    public class BlogContext : DbContext, IBlogContext
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

            //modelBuilder.Entity<Author>().HasData(BlogContextSeeds.Authors);
            //modelBuilder.Entity<Post>().HasData(BlogContextSeeds.Posts);
            //modelBuilder.Entity<Post>().OwnsOne(p => p.Tags).HasData(BlogContextSeeds.Tags);
            //modelBuilder.Entity<Comment>().HasData(BlogContextSeeds.Comments);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (new DateTime(1, 1, 1, 0, 0, 0) >= entry.Entity.Created)
                            entry.Entity.Created = DateTime.Now;
                        entry.Entity.Modified = entry.Entity.Created;
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