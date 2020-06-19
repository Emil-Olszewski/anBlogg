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
        private readonly ExpandoObject expandoObject = new ExpandoObject();
        private readonly Mock<IProperties> properties = new Mock<IProperties>();
        private readonly Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
        private readonly JsonSerializerOptions options = new JsonSerializerOptions();

        [Test()]
        public void CreatePagesLinksTest()
        {
            #region Arrange

            var elements = new List<string>() { "1", "2", "3" }.AsQueryable();
            var pagedList = PagedList<string>.Create(elements, 2, 1);
            var target = new Pagination(properties.Object);
            var parameters = new PostResourceParameters();

            PrepareObjects();

            #endregion Arrange

            #region Act

            var result = target.CreatePagesLinks(pagedList,
                parameters, new ResourceUriHelper(urlHelper.Object));

            #endregion Act

            #region Assert

            Assert.IsTrue(result.HasNext);
            Assert.IsTrue(result.HasPrevious);
            Assert.AreEqual("GetStrings", result.Next);

            #endregion Assert
        }

        private void PrepareObjects()
        {
            urlHelper.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns((string x, object y) => x);

            properties.Setup(p => p.CreateDynamicResourceFrom(It.IsAny<IResourceParameters>()))
                .Returns(expandoObject);

            expandoObject.TryAdd("PageNumber", 3);

            options.PropertyNameCaseInsensitive = true;
        }
    }
}