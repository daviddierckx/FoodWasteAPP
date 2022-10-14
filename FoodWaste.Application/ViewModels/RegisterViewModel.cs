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
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
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
        public string TelefoonNummer { get; set; }
        [Required]
        public DateTime Geboortedatum { get; set; }
        [Required]
        [Display(Name = "Studie stad")]
        public Stad? StudieStad { get; set; }
    }
}
