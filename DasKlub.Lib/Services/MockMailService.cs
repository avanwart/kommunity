using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasKlub.Lib.Services
{
    public class MockMailService : IMailService
    {
        public bool SendMail(string fromEmail, string toEmail, string subject, string body)
        {
            Debug.WriteLine(string.Concat("Sendmail: ", subject));
            return true;
        }
    }
}
