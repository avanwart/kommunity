using System;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Resources;

namespace DasKlub.Lib.Services
{
    public class MailService : IMailService
    {
        /// <summary>
        /// Sends an email with opt out option
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
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
                
                var amzClient = new AmazonSimpleEmailServiceClient(
                    AmazonCloudConfigs.AmazonAccessKey, AmazonCloudConfigs.AmazonSecretKey);
                var dest = new Destination();
                dest.ToAddresses.Add(toEmail);

                body =
                string.Format("{0}{1}-----------------------------------------------{1}{1}{2}{1}{1}{1}-----------------------------------------------{1}{3}{1}{1}{4}{1}{1}{0}",
                Messages.DoNotRespondToThisEmail, 
                Environment.NewLine, 
                body, 
                Messages.EditYourEmailSettings, 
                GeneralConfigs.EmailSettingsURL);

                var bdy = new Body { Text = new Content(body) };
                var title = new Content(subject);
                var message = new Message(title, bdy);
                var ser = new SendEmailRequest(fromEmail, dest, message);
 
                amzClient.SendEmailAsync(ser);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
