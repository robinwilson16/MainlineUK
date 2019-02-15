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
    public class ContactModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public ContactModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public StocklistImport StocklistImport { get; set; }

        public void OnGet(int? id)
        {
            FormSubmitted = false;
            EmailSent = false;

            StocklistImport = _context.StocklistImport
                .FirstOrDefault(s => s.StocklistImportID == id);
        }

        [BindProperty]
        public ContactAboutVehicle ContactAboutVehicle { get; set; }

        public bool FormSubmitted { get; set; }
        public bool EmailSent { get; set; }

        public void OnPost(int? id)
        {
            ContactAboutVehicle.StocklistImportID = id;

            if (ModelState.IsValid)
            {
                StocklistImport = _context.StocklistImport
                    .FirstOrDefault(s => s.StocklistImportID == id);

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
                    <h2 style=""color: #000080;""><a href=""https://www.mainlineuk.co.uk/Stock/Details/" + StocklistImport.StocklistImportID + @""" rel=""noopener"">" + StocklistImport.Make + @" " + StocklistImport.Model + @"</a></h2>
                    <h3 style=""color: #1e90ff;""><a href=""https://www.mainlineuk.co.uk/Stock/Details/" + StocklistImport.StocklistImportID + @""" rel=""noopener"">" + StocklistImport.Derivative + @" (" + StocklistImport.ManufacturedYear + @")</a></h3>
                    <h3 style=""color: #1e90ff;"">&pound;" + StocklistImport.Price + @"</h3>
                    <hr stle=""border: 2px solid #1e90ff;"" />
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
                    " + ContactAboutVehicle.Name + @"
                    </td>
                    <td>
                    Telephone
                    </td>
                    <td>
                    <a href=""tel:" + ContactAboutVehicle.Telephone + @""">" + ContactAboutVehicle.Telephone + @"</a>
                    </td>
                    </tr>

                    <tr>
                    <td>
                    Email
                    </td>
                    <td colspan=""3"">
                    <a href=""mailto:" + ContactAboutVehicle.Email + @"?subject=Website Enquiry"">" + ContactAboutVehicle.Email + @"</a>
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
                    " + ContactAboutVehicle.Enquiry + @"
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
