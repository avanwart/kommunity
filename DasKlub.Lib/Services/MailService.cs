using System;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Resources;

namespace DasKlub.Lib.Services
{
    public class MailService : IMailService
    {
        /// <summary>
        ///     Sends an email with opt out option
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
                    AmazonCloudConfigs.AmazonAccessKey, AmazonCloudConfigs.AmazonSecretKey, RegionEndpoint.USEast1);
                var dest = new Destination();
                dest.ToAddresses.Add(toEmail);

                body =
                    Messages.DoNotRespondToThisEmail +
                    Environment.NewLine +
                    Environment.NewLine +
                    "----------------------------" +
                    Environment.NewLine +
                    body +
                    Environment.NewLine +
                    Environment.NewLine +
                    Environment.NewLine +
                    Environment.NewLine +
                    Environment.NewLine +
                    "----------------------------" +
                    Environment.NewLine +
                    Messages.EMail + " " + Messages.Settings +
                    Environment.NewLine +
                    GeneralConfigs.EmailSettingsURL + " :" +
                    Environment.NewLine +
                    Environment.NewLine +
                    Messages.DoNotRespondToThisEmail +
                    Environment.NewLine;

                var bdy = new Body {Text = new Content(body)};
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