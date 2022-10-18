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
        public string BeschrijvendeNaam { get; set; }
        [ForeignKey("Product")]
        public string SelectedProductId { get; set; }
        //TODO Change to ICOLLECTION?
        // public List<Product> Producten { get; set; }
        [NotMapped]
        public ICollection<Product> ProductCollectie { get; set; }

        public string Stad { get;set; }
        public string Kantine { get; set; }
        public DateTime TijdOphalen { get; set; }
        public DateTime TijdTotOphalen { get; set; }
        public bool Meerderjarig { get; set; }
        public int Prijs {get; set; }
        public string TypeMaaltijd { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? GereserveerdDoor { get; set; }

    }
  
}
