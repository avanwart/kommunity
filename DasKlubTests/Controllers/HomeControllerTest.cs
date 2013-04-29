using System.Web.Mvc;
using DasKlub.Controllers;
using DasKlub.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlubTests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeContact_Invoked_IsNotNull()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}