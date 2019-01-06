using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainlineUK.Models
{
    public class CarRequestSell : CarRequest
    {
        public int CarRequestSellID { get; set; }

        [Display(Name = "Registration*", Prompt = "Registration*")]
        [StringLength(8)]
        [Required(ErrorMessage = "Please enter your registration number")]
        public string CarRegistration { get; set; }
    }
}
