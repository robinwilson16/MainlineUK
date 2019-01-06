using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MainlineUK.Models;
using MainlineUK.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainlineUK.Pages
{
    public class RequestCarModel : PageModel
    {
        public void OnGet()
        {
            FormSubmitted = false;
            EmailSent = false;
        }

        [BindProperty]
        public CarRequestBuy CarRequestBuy { get; set; }

        public bool FormSubmitted { get; set; }
        public bool EmailSent { get; set; }

        public IEnumerable<SelectListItem> FuelType = Enum.GetValues(typeof(FuelType))
            .Cast<FuelType>()
            .Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = Enums.GetDescription(t)
            });

        public IEnumerable<SelectListItem> Transmission = Enum.GetValues(typeof(Transmission))
            .Cast<Transmission>()
            .Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = Enums.GetDescription(t)
            });

        public void OnPost()
        {
            string EmailSubject;
            string EmailBody;

            EmailSubject = "Request a car on Mainline UK";

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
                <h2 style=""color: #000080;"">Sell Your Car</h2>
                <h3 style=""color: #1e90ff;"">Personal Details</h3>
                <hr stle=""border: 2px solid #1e90ff;"" />
                </td>
                </tr>

                <tr>
                <td>
                Name
                </td>
                <td>
                " + CarRequestBuy.Name + @"
                </td>
                <td>
                Telephone
                </td>
                <td>
                <a href=""tel:" + CarRequestBuy.Telephone + @""">" + CarRequestBuy.Telephone + @"</a>
                </td>
                </tr>

                <tr>
                <td>
                Email
                </td>
                <td colspan=""3"">
                <a href=""mailto:" + CarRequestBuy.Email + @"?subject=Sell Your Car with Mainline UK"">" + CarRequestBuy.Email + @"</a>
                </td>
                </tr>

                <tr>
                <td colspan=""4"">
                <h3 style=""color: #1e90ff;"">Your Car Details</h3>
                <hr stle=""border: 2px solid #1e90ff;"" />
                </td>
                </tr>

                <tr>
                <td>
                Make
                </td>
                <td>
                " + CarRequestBuy.CarMake + @"
                </td>
                <td>
                Model
                </td>
                <td>
                " + CarRequestBuy.CarModel + @"
                </td>
                </tr>

                <tr>
                <td>
                Owners
                </td>
                <td>
                " + CarRequestBuy.CarOwners + @"
                </td>
                <td>
                Mileage
                </td>
                <td>
                " + CarRequestBuy.CarMileage + @"
                </td>
                </tr>

                <tr>
                <td>
                Colour
                </td>
                <td>
                " + CarRequestBuy.CarColour + @"
                </td>
                <td>
                Fuel Type
                </td>
                <td>
                " + CarRequestBuy.CarFuelType + @"
                </td>
                </tr>

                <tr>
                <td>
                Transmission
                </td>
                <td>
                " + CarRequestBuy.CarTransmission + @"
                </td>
                <td colspan=""2"">
                &nbsp;
                </td>
                </tr>

                <tr>
                <td>
                Condition/Damage Report
                </td>
                <td colspan=""3"">
                " + CarRequestBuy.CarCondition + @"
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