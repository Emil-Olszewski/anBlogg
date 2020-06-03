using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Implementations;
using anBlogg.Application.Services.Models;
using anBlogg.WebApi.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;

namespace anBlogg.ApplicationTests
{
    [TestFixture()]
    public class PaginationTests
    {
        private class ReturnedMetadata
        {
            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public string PreviousPageLink { get; set; }
            public string NextPageLink { get; set; }
        }

        private readonly ExpandoObject expandoObject = new ExpandoObject();
        private readonly Mock<IProperties> properties = new Mock<IProperties>();
        private readonly Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
        private readonly JsonSerializerOptions options = new JsonSerializerOptions();

        [Test()]
        public void CreateHeaderTest()
        {
            #region Arrange

            var elements = new List<string>() { "1", "2", "3" }.AsQueryable();
            var pagedList = PagedList<string>.Create(elements, 2, 1);
            var parameters = new PostResourceParameters();

            var target = new Pagination(properties.Object);

            PrepareObjects();

            #endregion Arrange

            #region Act

            var result = target.CreateHeader(pagedList, parameters, urlHelper.Object);
            var deserialized = JsonSerializer
                .Deserialize<ReturnedMetadata>(result.Value, options);

            #endregion Act

            #region Assert

            Assert.IsInstanceOf(typeof(Header), result);
            Assert.AreEqual("GetStrings", deserialized.PreviousPageLink);

            #endregion Assert
        }

        private void PrepareObjects()
        {
            properties.Setup(p => p.CreateDynamicResourceFrom(It.IsAny<IResourceParameters>()))
                .Returns(expandoObject);

            urlHelper.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns((string x, object y) => x);

            expandoObject.TryAdd("PageNumber", 3);

            options.PropertyNameCaseInsensitive = true;
        }
    }
}