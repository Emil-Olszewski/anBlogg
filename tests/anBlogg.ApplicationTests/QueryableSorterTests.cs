using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Implementations;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.ApplicationTests
{
    [TestFixture()]
    public class QueryableSorterTests
    {
        private class TestPerson
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int FootSize { get; set; }


            public TestPerson(string name, int age)
            {
                Name = name;
                Age = age;
                FootSize = 30;
            }
        }

        [Test()]
        public void ApplySortTest()
        {
            #region Arrange

            var properties = new Mock<Properties>();
            var source = new List<TestPerson>()
            {
                new TestPerson("Janek", 30),
                new TestPerson("Agata", 28),
                new TestPerson("Mirek", 17),
                new TestPerson("Bercio", 24)
            }.AsQueryable();

            var arguedProperty1 = "age asc";
            var arguedProperty2 = "name desc";

            var mappingDictionary = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Age", new PropertyMappingValue(new List<string>() { "Age" } ) },
                { "Name", new PropertyMappingValue(new List<string>() { "Name" } ) },
            };

            var target = new QueryableSorter(properties.Object);

            #endregion Arrange

            #region Act

            var result1 = target.ApplySort(source, arguedProperty1, mappingDictionary).ToList();
            var result2 = target.ApplySort(source, arguedProperty2, mappingDictionary).ToList();

            #endregion Act

            #region Assert

            Assert.AreEqual("Mirek", result1[0].Name);
            Assert.AreEqual("Janek", result1[^1].Name);

            Assert.AreEqual("Mirek", result2[0].Name);
            Assert.AreEqual("Agata", result2[^1].Name);

            #endregion Assert
        }
    }
}