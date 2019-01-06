using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainlineUK.Models
{
    public class CarRequestBuy : CarRequest
    {
        public int CarRequestBuyID { get; set; }

        [Display(Name = "Owners*", Prompt = "Owners*")]
        [Required(ErrorMessage = "Please enter number of previous owners")]
        public int CarOwners { get; set; }
    }
}
