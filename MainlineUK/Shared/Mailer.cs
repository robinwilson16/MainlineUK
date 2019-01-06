using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MainlineUK.Shared
{
    public class Mailer
    {
        public static bool SendMail(string EmailFrom, string EmailTo, string EmailCC, string EmailBCC, string EmailSubject, string EmailBody)
        {
            bool isComplete = true;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            IConfiguration Configuration = Startup.Configuration.GetSection("EmailServer");

            string EmailHost = Configuration["Host"];
            string EmailUsername = Configuration["Username"];
            string EmailPassword = Configuration["Password"];
            string EmailPort = Configuration["Port"];
            string EmailUseSSL = Configuration["UseSSL"];

            try
            {
                smtpClient.Host = EmailHost;
                smtpClient.Port = Int32.Parse(EmailPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(EmailUsername, EmailPassword);
                smtpClient.EnableSsl = bool.Parse(EmailUseSSL);

                message.From = new MailAddress(EmailFrom);
                message.To.Add(EmailTo);
                message.Subject = EmailSubject;

                if (EmailCC != null && EmailCC != "") { message.Bcc.Add(new MailAddress(EmailCC)); }
                if (EmailBCC != null && EmailBCC != "") { message.Bcc.Add(new MailAddress(EmailBCC)); }

                message.IsBodyHtml = true;

                string html = EmailBody;

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, new ContentType("text/html"));

                message.AlternateViews.Add(htmlView);

                smtpClient.Send(message);
            }
            catch
            {
                isComplete = false;
            }

            return isComplete;
        }
    }
}
