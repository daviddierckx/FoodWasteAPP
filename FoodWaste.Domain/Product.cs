using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string BeschrijvendeNaam { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public bool Alcohol { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Foto { get; set; }
    }
}
