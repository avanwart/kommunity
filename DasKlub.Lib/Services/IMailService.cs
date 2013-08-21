namespace DasKlub.Lib.Services
{
    public interface IMailService
    {
        bool SendMail(string fromEmail, string toEmail, string subject, string body);
    }
}
