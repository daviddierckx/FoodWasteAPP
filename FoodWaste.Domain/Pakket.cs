using FoodWaste.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Domain
{
    public class Pakket
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string BeschrijvendeNaam { get; set; }
        [ForeignKey("Product")]
        [Required(ErrorMessage = "Please select at least one product")]
        public string? SelectedProductId { get; set; }
        //TODO Change to ICOLLECTION?
        // public List<Product> Producten { get; set; }
        [NotMapped]
        [Required, MinLength(1, ErrorMessage = "At least one item required")]
        public ICollection<Product>? ProductCollectie { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public string Stad { get;set; }
        [Required(ErrorMessage = "This field is required")]
        public string Kantine { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public DateTime TijdOphalen { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public DateTime TijdTotOphalen { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public bool Meerderjarig { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Prijs {get; set; }
        public string TypeMaaltijd { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? GereserveerdDoor { get; set; }

        public string? StudentenIds { get; set; }
        public Student? Student { get; set; }
    }

}
