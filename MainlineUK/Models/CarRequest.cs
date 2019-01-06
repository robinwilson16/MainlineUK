using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainlineUK.Models
{
    public enum FuelType
    {
        Petrol,
        Diesel,
        Hybrid,
        Electric,
        [Display(Name = "LG Gas")]
        LG_Gas
    }

    public enum Transmission
    {
        Automatic,
        Manual
    }

    public class CarRequest
    {
        public int CarRequestID { get; set; }

        [Display(Name = "Name*", Prompt = "Name*")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Display(Name = "Telephone*", Prompt = "Telephone*")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15)]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string Telephone { get; set; }

        [Display(Name = "Email*", Prompt = "Email*")]
        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }

        [Display(Name = "Make*", Prompt = "Make*")]
        [StringLength(20)]
        [Required(ErrorMessage = "Please enter a make")]
        public string CarMake { get; set; }

        [Display(Name = "Model*", Prompt = "Model*")]
        [StringLength(20)]
        [Required(ErrorMessage = "Please enter a model")]
        public string CarModel { get; set; }

        [Display(Name = "Mileage*", Prompt = "Mileage*")]
        [Required(ErrorMessage = "Please enter your current mileage")]
        public int CarMileage { get; set; }

        [Display(Name = "Colour*", Prompt = "Colour*")]
        [StringLength(20)]
        [Required(ErrorMessage = "Please enter the colour of your vehicle")]
        public string CarColour { get; set; }

        [Display(Name = "Fuel Type*", Prompt = "Fuel Type*")]
        [StringLength(10)]
        [BindRequired]
        [Required(ErrorMessage = "Please enter the fuel type your vehicle uses")]
        public string CarFuelType { get; set; }

        [Display(Name = "Transmission*", Prompt = "Transmission*")]
        [StringLength(10)]
        [BindRequired]
        [Required(ErrorMessage = "Please enter your transmission")]
        public string CarTransmission { get; set; }

        [Display(Name = "Condition/Damage Report", Prompt = "Condition/Damage Report")]
        public string CarCondition { get; set; }
    }
}
