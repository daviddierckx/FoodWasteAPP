using FoodWaste.Application.Validators.CustomValidation;
using FoodWaste.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage ="Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,16}$", ErrorMessage = "Password not strong enough.")]
        public string Password { get; set; }
        [Display(Name ="Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        [Display(Name = "Telefoon Nummer")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string TelefoonNummer { get; set; }
        [Required]
        [MinumumLeeftijdAanmelden(ErrorMessage = "De leeftijd bij aanmelden voor een account is minimaal 16 jaar.")]
        public DateTime Geboortedatum { get; set; }
        [Required]
        [Display(Name = "Studie stad")]
        public Stad? StudieStad { get; set; }

    }
}
