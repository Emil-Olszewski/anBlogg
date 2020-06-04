using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Implementations;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.ApplicationTests
{
    [TestFixture()]
    public class PropertiesTests
    {
        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }

            public Person(string firstName, string lastName, int age)
            {
                FirstName = firstName;
                LastName = lastName;
                Age = age;
            }
        }

        [Test()]
        public void ExistsInTest()
        {
            #region Arrange

            var propertiesSet1 = "firstname, age";
            var propertiesSet2 = "age, nationality";
            var target = new Properties();

            #endregion Arrange

            #region Act

            var result1 = target.ExistsIn<Person>(propertiesSet1);
            var result2 = target.ExistsIn<Person>(propertiesSet2);

            #endregion Act

            #region Assert

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);

            #endregion Assert
        }

        [Test()]
        public void ShapeDataTest()
        {
            #region Arrange

            var properties = "firstname, age";
            var source = new List<Person>()
            {
                new Person("Pawel", "Krosniak", 20),
                new Person("Horacy", "Czeresniak", 24),
                new Person("Mohamed", "Bombowy", 29),
            };

            var target = new Properties();

            #endregion Arrange

            #region Act

            var result = target.ShapeData(source, properties).ToList();

            #endregion Act

            #region Assert

            var sampleResult = (IDictionary<string, object>)result[0];

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(sampleResult.ContainsKey("FirstName"));
            Assert.AreEqual(source[0].FirstName, sampleResult["FirstName"]);
            Assert.IsTrue(sampleResult.ContainsKey("Age"));
            Assert.AreEqual(source[0].Age, sampleResult["Age"]);
            Assert.IsFalse(sampleResult.ContainsKey("LastName"));

            #endregion Assert
        }

        [Test()]
        public void GetPropertiesNamesFromTest()
        {
            #region Arrange

            var arguedPropertiesSet = "name desc, age, nationality asc";
            var target = new Properties();

            #endregion Arrange

            #region Act

            var result = target.GetPropertiesNamesFrom(arguedPropertiesSet);

            #endregion Act

            #region Assert

            var expected = new List<string>() { "name", "age", "nationality" };

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(expected, result);

            #endregion Assert
        }

        [Test()]
        public void GetPropertiesNameWithArgumentsFromTest()
        {
            #region Arrange

            var arguedPropertiesSet = "name desc, age, nationality asc";
            var target = new Properties();

            #endregion Arrange

            #region Act

            var result = target.GetPropertiesNamesWithArgumentsFrom(arguedPropertiesSet).ToList();

            #endregion Act

            #region Assert

            var expected = new List<ArguedProperty>()
            {
                new ArguedProperty("name", "desc"),
                new ArguedProperty("age"),
                new ArguedProperty("nationality", "asc")
            };

            Assert.AreEqual(3, result.Count());
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i].PropertyName, result[i].PropertyName);
                Assert.AreEqual(expected[i].Argument, result[i].Argument);
            }

            #endregion Assert
        }

        [Test()]
        public void CreateDynamicResourceFromTest()
        {
            #region Arrange

            var testPerson = new Person("Matt", "Kowalski", 15);
            var target = new Properties();

            #endregion Arrange

            #region Act

            var result = (IDictionary<string, object>)target
                .CreateDynamicResourceFrom(testPerson);

            #endregion Act

            #region Assert

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.ContainsKey("FirstName"));
            Assert.AreEqual(testPerson.FirstName, result["FirstName"]);
            Assert.IsTrue(result.ContainsKey("Age"));
            Assert.AreEqual(testPerson.Age, result["Age"]);
            Assert.IsTrue(result.ContainsKey("LastName"));
            Assert.AreEqual(testPerson.LastName, result["LastName"]);

            #endregion Assert
        }
    }
}