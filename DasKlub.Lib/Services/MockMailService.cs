using System.Diagnostics;

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