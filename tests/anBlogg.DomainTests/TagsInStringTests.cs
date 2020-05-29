using anBlogg.Domain.Services.Implementations;
using NUnit.Framework;
using System.Linq;

namespace anBlogg.DomainTests
{
    [TestFixture()]
    public class TagsInStringTests
    {
        [Test()]
        public void EnumerateTest()
        {
            // Arrange
            var target = new TagsInString();

            // Act
            var result = target.Enumerate(" <tag1> <tag2><tag3> ").ToList();

            // Assert
            Assert.AreEqual("tag1", result[0]);
            Assert.AreEqual("tag3", result[2]);
        }

        [Test()]
        public void EnumerateWithBracketsTest()
        {
            // Arrange
            var target = new TagsInString();

            // Act
            var result = target.EnumerateWithBrackets("<tag1><tag2> <tag3> ").ToList();

            // Assert
            Assert.AreEqual("<tag1>", result[0]);
            Assert.AreEqual("<tag3>", result[2]);
        }

        [Test()]
        public void InsertIntoAndReturnTest()
        {
            // Arrange
            var target = new TagsInString();

            // Act
            var result = target.InsertInto("<tag1> ", "tag2 ", " tag3 ");

            // Assert
            Assert.AreEqual("<tag1><tag2><tag3>", result);
        }

        [Test()]
        public void RemoveFromAndReturnTest()
        {
            // Arrange
            var target = new TagsInString();

            // Act
            var result = target.RemoveFrom(" <tag1> <tag2> <tag3>", "tag2 ");

            // Assert
            Assert.AreEqual("<tag1><tag3>", result);
        }
    }
}