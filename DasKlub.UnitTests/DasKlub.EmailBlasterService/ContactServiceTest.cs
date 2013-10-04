using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DasKlub.EmailBlasterService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlub.UnitTests.DasKlub.EmailBlasterService
{
    [TestClass]
    public class ContactServiceTest
    {
        [TestMethod]
        public void QuartzScheduler_IsUserBirthday_SendsBirthdayEmail()
        {
            // arrange
            var service = new ContactService();
            // TODO: POPULATE A MOCK OF USERACCOUNTS WITH A BIRTHDAY USER
            
            // act
            service.OnDebug();

            // assert

        }
    }
}
