using anBlogg.Application.Services.Implementations;
using anBlogg.Domain.Entities;
using anBlogg.Domain.Services;
using anBlogg.Domain.ValueObjects;
using anBlogg.Infrastructure.Persistence;
using anBlogg.WebApi.ResourceParameters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.ApplicationTests
{
    [TestFixture()]
    public class BlogRepositoryTests
    {
        private List<Guid> guids;
        private List<Post> posts;
        private List<Author> authors;
        private Mock<ITagsInString> tagsInString;

        public BlogRepositoryTests()
        {
            PopulateGuids();
            PopulatePosts();
            PopulateAuthors();
            SetupTagsInString();
        }

        [Test()]
        public void GetAllPostWithRequiredTagsTest()
        {
            #region Arrange
            var connection = SetConnection();
            var options = GetOptionsFor(connection);
            PrepareDatabaseWith(options);

            var parameters = new PostResourceParameters()
            {
                RequiredTags = "<tag2><tag3>",
            };

            using var context = new BlogContext(options);
            var target = new BlogRepository(context, tagsInString.Object);
            #endregion

            #region Act
            var result = target.GetPosts(parameters).ToList();
            #endregion

            #region Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Post3", result[0].Title);
            Assert.AreEqual("Post1", result[1].Title);
            #endregion

        }

        [Test()]
        public void GetAllPostsForAuthorWithRequiredTagsTest()
        {
            #region Arrange 
            var connection = SetConnection();
            var options = GetOptionsFor(connection);
            PrepareDatabaseWith(options);

            var parameters1 = new PostResourceParameters()
            {
                RequiredTags = "<tag2><tag4>",
            };

            var parameters2 = new PostResourceParameters()
            {
                RequiredTags = "<unexisting-tag>",
            };

            using var context = new BlogContext(options);
            var target = new BlogRepository(context, tagsInString.Object);
            #endregion

            #region Act
            var result1 = target.GetAllPostsForAuthor(guids[1], parameters1).ToList();
            var result2 = target.GetAllPostsForAuthor(guids[1], parameters2).ToList();
            #endregion

            #region Assert
            Assert.AreEqual(1, result1.Count());
            Assert.AreEqual(0, result2.Count());
            Assert.AreEqual("Post4", result1[0].Title);
            #endregion
        }

        [Test()]
        public void GetAllPostsPaginatedTest()
        {
            #region Arrange 
            var connection = SetConnection();
            var options = GetOptionsFor(connection);
            PrepareDatabaseWith(options);

            var parameters = new PostResourceParameters()
            {
                PostsDisplayed = 2,
                PageNumber = 2
            };

            using var context = new BlogContext(options);
            var target = new BlogRepository(context, tagsInString.Object);
            #endregion
           
            #region Act
            var result = target.GetPosts(parameters).ToList();
            var result2 = target.GetPosts(new PostResourceParameters()).ToList();
            #endregion

            #region Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Post2", result[0].Title);
            #endregion
        }
      
        private void PopulateGuids()
        {
            guids = new List<Guid>();
            for (int i = 0; i < 10; i++)
                guids.Add(Guid.NewGuid());
        }

        private void PopulatePosts()
        {
            posts = new List<Post>()
            {
                new Post
                {
                    Title = "Post1",
                    Tags = new Tags("<tag1><tag2><tag3><tag4>"),
                    AuthorId = guids[0],
                    Created =  new DateTime(2013, 10, 10, 12, 32, 00)
                },
                new Post
                {
                    Title = "Post2",
                    Tags = new Tags("<tag1><tag3><tag4>"),
                    AuthorId = guids[1],
                    Created =  new DateTime(2013, 10, 10, 12, 33, 00)
                },
                new Post
                {
                    Title = "Post3",
                    Tags = new Tags("<tag2><tag3><tag4>"),
                    AuthorId = guids[0],
                    Created =  new DateTime(2013, 10, 10, 12, 34, 00)
                },
                new Post
                {
                    Title = "Post4",
                    Tags = new Tags("<tag2><tag4>"),
                    AuthorId = guids[1],
                    Created =  new DateTime(2013, 10, 10, 12, 35, 00)
                }
            };
        }

        private void PopulateAuthors()
        {
            authors = new List<Author>()
            {
                new Author { Id = guids[0] },
                new Author { Id = guids[1] }
            };
        }

        private void PrepareDatabaseWith(DbContextOptions<BlogContext> options)
        {
            using var context = new BlogContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            context.Posts.AddRange(posts);
            context.Authors.AddRange(authors);
            context.SaveChanges();
        }

        private SqliteConnection SetConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder 
                { DataSource = ":memory:" };
            return new SqliteConnection(connectionStringBuilder.ToString());
        }

        private DbContextOptions<BlogContext> GetOptionsFor(SqliteConnection connection)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<BlogContext>().UseSqlite(connection);
            return optionsBuilder.Options;
        }

        private void SetupTagsInString()
        {
            tagsInString = new Mock<ITagsInString>();

            tagsInString.Setup(t => t.EnumerateWithBrackets("<tag2><tag3>"))
                .Returns(new List<string>() { "<tag2><tag3>" });

            tagsInString.Setup(t => t.EnumerateWithBrackets("<tag2><tag4>"))
                .Returns(new List<string>() { "<tag2><tag4>" });

            tagsInString.Setup(t => t.EnumerateWithBrackets("<unexisting-tag>"))
                .Returns(new List<string>() { "<unexisting-tag>" });
        }
    }
}