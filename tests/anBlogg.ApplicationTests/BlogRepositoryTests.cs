using anBlogg.Application.Services.Implementations;
using anBlogg.Domain.Entities;
using anBlogg.Domain.Services;
using anBlogg.Domain.ValueObjects;
using anBlogg.Infrastructure.Persistence;
using anBlogg.WebApi.ResourceParameters;
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
        [Test()]
        public void GetAllPostsTest()
        {
            #region Arrange 
            var posts = new List<Post>()
            {
                new Post
                {
                    Title = "Post1",
                    Tags = new Tags("<tag1><tag2><tag3><tag4>")
                },
                new Post
                {
                    Title = "Post2",
                    Tags = new Tags("<tag1><tag3><tag4>")
                },
                new Post
                {
                    Title = "Post3",
                    Tags = new Tags("<tag2><tag3><tag4>")
                },
                new Post
                {
                    Title = "Post4",
                    Tags = new Tags("<tag2><tag4>")
                }
            };

            var parameters = new PostResourceParameters()
            {
                RequiredTags = "<tag2><tag3>",
            };

            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase($"TestingDatabase{Guid.NewGuid()}")
                .Options;

            using var context = new BlogContext(options);
            context.Posts.AddRange(posts);
            context.SaveChanges();

            var tagsInString = new Mock<ITagsInString>();
            SetupTagsInString(parameters, tagsInString);

            using var otherContext = new BlogContext(options);
            var target = new BlogRepository(otherContext, tagsInString.Object);
            #endregion

            #region Act
            var result = target.GetAllPosts(parameters).ToList();
            #endregion

            #region Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Post1", result[0].Title);
            Assert.AreEqual("Post3", result[1].Title);
            #endregion
        }

        [Test()]
        public void GetAllPostsForAuthor()
        {
            #region Arrange 
            var posts = new List<Post>()
            {
                new Post
                {
                    AuthorId = Guid.Parse("194888ce-a49e-423b-a412-c60bc4073538"),
                    Title = "Post1",
                    Tags = new Tags("<tag1><tag2><tag3>")
                },
                new Post
                {
                    AuthorId = Guid.Parse("3f53bb14-e42d-43d7-bb9d-79c22bf91076"),
                    Title = "Post2",
                    Tags = new Tags("<tag1><tag3><tag4>")
                },
                new Post
                {
                    AuthorId = Guid.Parse("194888ce-a49e-423b-a412-c60bc4073538"),
                    Title = "Post3",
                    Tags = new Tags("<tag1><tag2><tag4>")
                },
                new Post
                {
                    AuthorId = Guid.Parse("3f53bb14-e42d-43d7-bb9d-79c22bf91076"),
                    Title = "Post4",
                    Tags = new Tags("<tag2><tag4>")
                }
            };

            var parameters = new PostResourceParameters()
            {
                RequiredTags = "<tag2><tag3>",
            };

            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase($"TestingDatabase{Guid.NewGuid()}")
                .Options;

            using var context = new BlogContext(options);
            context.Posts.AddRange(posts);
            context.SaveChanges();

            var tagsInString = new Mock<ITagsInString>();
            SetupTagsInString(parameters, tagsInString);

            using var otherContext = new BlogContext(options);
            var target = new BlogRepository(otherContext, tagsInString.Object);
            #endregion

            #region Act
            var result = target.GetAllPostsForAuthor
                (Guid.Parse("194888ce-a49e-423b-a412-c60bc4073538"), parameters).ToList();
            #endregion

            #region Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Post1", result[0].Title);
            #endregion
        }

        private static void SetupTagsInString
            (PostResourceParameters parameters, Mock<ITagsInString> tagsInString)
        {
            tagsInString.Setup(t => t.EnumerateWithBrackets(parameters.RequiredTags))
                .Returns(new List<string>() { "<tag2><tag3>" });
        }
    }
}