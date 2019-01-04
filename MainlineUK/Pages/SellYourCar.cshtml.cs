using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainlineUK.Pages
{
    public class SellYourCarModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPost()
        {
            SmtpClient client = new SmtpClient("rwshosting.com");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("enquiries@mainlineuk.co.uk", "Sonyvaio1");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("enquiries@mainlineuk.co.uk");
            mailMessage.To.Add("enquiries@mainlineuk.co.uk");
            mailMessage.Body = "body123";
            mailMessage.Subject = "subject123";
            client.Send(mailMessage);
        }
    }
}