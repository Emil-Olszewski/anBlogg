using NUnit.Framework;
using AutoMapper;
using anBlogg.WebApi.Profiles;
using anBlogg.Domain.Entities;
using anBlogg.Domain.ValueObjects;
using anBlogg.WebApi.Models;

namespace anBlogg.WebApiTests
{
    [TestFixture()]
    public class ProfilesTests
    {
        [Test()]
        public void PostsProfileTest()
        {
            // Arrange
            var mapperConfiguration
                = new MapperConfiguration(cfg => cfg.AddProfile(new PostsProfile()));
            var target = new Mapper(mapperConfiguration);

            var postInput = new PostInputDto()
            {
                Contents = "Contents",
                Tags = new string [] { "tag1", "tag2", "tag3" }
            };

            var post = new Post()
            {
                Tags = new Tags("tag1", "tag2", "tag3")
            };

            // Act 
            var testDelegate = new TestDelegate(mapperConfiguration.AssertConfigurationIsValid);
            var inputResult = target.Map<Post>(postInput);
            var outputResult = target.Map<PostOutputDto>(post);

            // Assert
            Assert.DoesNotThrow(testDelegate);

            var expectedInputTags = "<tag1><tag2><tag3>";
            Assert.AreEqual(expectedInputTags, inputResult.Tags.Raw);
            Assert.AreEqual(postInput.Contents, inputResult.Contents);

            var expectedOutputTags = new string[] { "tag1", "tag2", "tag3" };
            Assert.AreEqual(expectedOutputTags, outputResult.Tags);
        }

        [Test()]
        public void AuthorsProfileTest()
        {
            // Arrange
            var mapperConfiguration
                = new MapperConfiguration(cfg => cfg.AddProfile(new AuthorsProfile()));

            // Act 
            var testDelegate = new TestDelegate(mapperConfiguration.AssertConfigurationIsValid);

            // Assert
            Assert.DoesNotThrow(testDelegate);
        }

        [Test()]
        public void CommentsProfileTest()
        {
            // Arrange
            var mapperConfiguration
                = new MapperConfiguration(cfg => cfg.AddProfile(new CommentsProfile()));

            // Act 
            var testDelegate = new TestDelegate(mapperConfiguration.AssertConfigurationIsValid);

            // Assert
            Assert.DoesNotThrow(testDelegate);
        }
    }
}