using System;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DasKlub.Lib.Configs;

namespace DasKlub.Lib.Services
{


    public class MailService : IMailService
    {
        public bool SendMail(string fromEmail, string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail) ||
                         string.IsNullOrEmpty(fromEmail) ||
                         string.IsNullOrEmpty(subject) ||
                         string.IsNullOrEmpty(body)) return false; 

            try
            {
                toEmail = toEmail.Trim();
                fromEmail = fromEmail.Trim();
                
                var amzClient = new AmazonSimpleEmailServiceClient(AmazonCloudConfigs.AmazonAccessKey, AmazonCloudConfigs.AmazonSecretKey);
                var dest = new Destination();
                dest.ToAddresses.Add(toEmail);
                var bdy = new Body { Text = new Content(body) };
                var title = new Content(subject);
                var message = new Message(title, bdy);
                var ser = new SendEmailRequest(fromEmail, dest, message);

                amzClient.SendEmail(ser);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
