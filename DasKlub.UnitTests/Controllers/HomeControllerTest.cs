using System.Web.Mvc;
using DasKlub.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlub.UnitTests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        /// <summary>
        /// TODO: MOCK THE MEMBERSHIP PROVIDER
        /// http://refriedgeek.blogspot.com/2012/07/unit-testing-static-methods-when-using.html
        /// </summary>
        [Ignore]
        [TestMethod]
        public void HomeContact_Invoked_IsNotNull()
        {
            // Arrange
            var controller = new HomeController( );

            // Act
            var result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}