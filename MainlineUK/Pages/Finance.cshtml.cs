using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainlineUK.Models;
using MainlineUK.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainlineUK.Pages
{
    public class FinanceModel : PageModel
    {
        public void OnGet()
        {
            FormSubmitted = false;
            EmailSent = false;
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public bool FormSubmitted { get; set; }
        public bool EmailSent { get; set; }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                string EmailSubject;
                string EmailBody;

                EmailSubject = "Vehicle Enquiry Form";

                EmailBody =
                    @"
                    <html>
                    <head>
                    <title>" + EmailSubject + @"</title>
                    </head>
                    <body>
                    <table border=""0"" cellspacing=""0"" cellpadding=""6"">
                    <tr>
                    <td colspan=""4"">
                    <img border=""0"" src=""https://www.mainlineuk.co.uk/images/MainlineUKLogo.png"" alt=""Mainline UK"" />
                    </td>
                    </tr>

                    <tr>
                    <td colspan=""4"">
                    <h2 style=""color: #000080;"">Personal Details</h2>
                    <hr stle=""border: 2px solid #1e90ff;"" />
                    </td>
                    </tr>

                    <tr>
                    <td>
                    Name
                    </td>
                    <td>
                    " + Contact.Name + @"
                    </td>
                    <td>
                    Telephone
                    </td>
                    <td>
                    <a href=""tel:" + Contact.Telephone + @""">" + Contact.Telephone + @"</a>
                    </td>
                    </tr>

                    <tr>
                    <td>
                    Email
                    </td>
                    <td colspan=""3"">
                    <a href=""mailto:" + Contact.Email + @"?subject=Mainline UK Finance Enquiry"">" + Contact.Email + @"</a>
                    </td>
                    </tr>

                    <tr>
                    <td colspan=""4"">
                    <h2 style=""color: #000080;"">Your Enquiry</h2>
                    <hr stle=""border: 2px solid #1e90ff;"" />
                    </td>
                    </tr>

                    <tr>
                    <td colspan=""4"">
                    " + Contact.Enquiry + @"
                    </td>
                    </tr>
                
                    <tr>
                    <td colspan=""4"">
                    <hr stle=""border: 2px solid #1e90ff;"" />
                    <h3 style=""color: #1e90ff;"">Someone will be in touch with you soon</h3>
                    <hr stle=""border: 2px solid #1e90ff;"" />
                    </td>
                    </tr>

                    </table>
                    </body>
                    </html>";

                string EmailFrom = "enquiries@mainlineuk.co.uk";
                string EmailTo = "enquiries@mainlineuk.co.uk";

                FormSubmitted = true;
                EmailSent = Mailer.SendMail(EmailFrom, EmailTo, null, null, EmailSubject, EmailBody);
            }
        }
    }
}